using AlbanWebApplication.Data;
using AlbanWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace AlbanWebApplication.Controllers
{
    public class ProfessionelsController : Controller
    {
        private readonly ProfessionelContext _context;

        public ProfessionelsController(ProfessionelContext context)
        {
            _context = context;
        }

        // GET: Professionels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Professionels.ToListAsync());
        }

        // GET: Professionels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professionels = await _context.Professionels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professionels == null)
            {
                return NotFound();
            }

            return View(professionels);
        }

        // GET: Professionels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Professionels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Photo,Description,Email,Phone,Siret")] Professionel professionels)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professionels);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professionels);
        }

        // GET: Professionels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professionels = await _context.Professionels.FindAsync(id);
            if (professionels == null)
            {
                return NotFound();
            }
            return View(professionels);
        }

        // POST: Professionels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Photo,Description,Email,Phone,Siret")] Professionel professionels)
        {
            if (id != professionels.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professionels);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessionelsExists(professionels.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(professionels);
        }

        // GET: Professionels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professionels = await _context.Professionels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professionels == null)
            {
                return NotFound();
            }

            return View(professionels);
        }

        // POST: Professionels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var professionels = await _context.Professionels.FindAsync(id);
            if (professionels != null)
            {
                _context.Professionels.Remove(professionels);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessionelsExists(int id)
        {
            return _context.Professionels.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportProfessionalsFromExcel()
        {
            // a mettre dans la requette par la suite
            string filePath = "C:\\Autojust\\Clean-Team-V2.xlsx";
            var professionels = GetProfessionelMapping(filePath);
            foreach(var kvp in professionels)
            {
                if (ModelState.IsValid)
                {
                    if( await _context.Professionels.FindAsync(kvp.Value.Id) == null)
                    {
                        _context.Add(kvp.Value);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return View(professionels);
        }
        private Dictionary<string, Professionel> GetProfessionelMapping(string path)
        {
            int headersRow = 1;
            int startingRow = 2;
            Dictionary<string, Professionel> excelData = new Dictionary<string, Professionel>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                // Accéder à la première feuille de calcul
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                var mappingDictionnary = CreateMappingDictionnary(worksheet, headersRow);

                for (int row = startingRow; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value == null)
                        break;

                    var key = worksheet.Cells[row, 1].Value.ToString();
                    var value = new Professionel();

                    // ici je peux faire un mapping entre colonne et nom des variables
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (mappingDictionnary[col] == "Name")
                            value.Name = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : String.Empty;

                        if (mappingDictionnary[col] == "Description")
                            value.Description = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : String.Empty;

                        if (mappingDictionnary[col] == "Adresse")
                            value.Adresse = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : String.Empty;

                        if (mappingDictionnary[col] == "Email")
                            value.Email = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : String.Empty;

                        if (mappingDictionnary[col] == "Phone")
                            value.Phone = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : String.Empty;

                        if (mappingDictionnary[col] == "Siret")
                            value.Siret = worksheet.Cells[row, col].Value != null ? worksheet.Cells[row, col].Value.ToString() : String.Empty;
                    }
                    excelData.TryAdd(key, value);
                }
            }
            return excelData;
        }
        private Dictionary<int, string> CreateMappingDictionnary(ExcelWorksheet worksheet, int headersRow)
        {
            var mappingDictionary = new Dictionary<int, string>();
            int colCount = worksheet.Dimension.Columns;
            for (int col = 1; col <= colCount; col++)
            {
                if (worksheet.Cells[headersRow, col].Value == null)
                    break;

                var key = col;
                var value = worksheet.Cells[headersRow, col].Value.ToString();
                mappingDictionary.TryAdd(key, value);
            }
            return mappingDictionary;
        }

        private string ChoseExcelFile()
        {
            throw new NotImplementedException();
        }
    }
}
