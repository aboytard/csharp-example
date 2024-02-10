// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int[,] matrix = {
            {1, 0, 1, 0, 0},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 0, 1},
            {0, 1, 1, 1, 0},
            {1, 1, 1, 1, 1}
        };

int maxSize = FindLargestSquareMatrix(matrix);

Console.WriteLine("La taille de la plus grande matrice carrée composée uniquement de 1 est : " + maxSize);

    static int FindLargestSquareMatrix(int[,] matrix)
{
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);

    int[,] dp = new int[rows, cols];
    int maxSize = 0;

    // Remplir la première ligne du tableau dp
    for (int i = 0; i < rows; i++)
    {
        dp[i, 0] = matrix[i, 0];
        maxSize = Math.Max(maxSize, dp[i, 0]);
    }

    // Remplir la première colonne du tableau dp
    for (int j = 0; j < cols; j++)
    {
        dp[0, j] = matrix[0, j];
        maxSize = Math.Max(maxSize, dp[0, j]);
    }

    // Remplir le reste du tableau dp
    for (int i = 1; i < rows; i++)
    {
        for (int j = 1; j < cols; j++)
        {
            if (matrix[i, j] == 1)
            {
                dp[i, j] = Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1])) + 1;
                maxSize = Math.Max(maxSize, dp[i, j]);
            }
        }
    }

    return maxSize;
}