using UnityEngine;
using System.Collections;

public class ImageProcessing
{

 


    
  
   

    public static byte[,] ApplyErrosion(byte[,] binaryMatrix,byte kernelSize)
    {
        int xSize = binaryMatrix.GetLength(0);
        int ySize = binaryMatrix.GetLength(1);
        byte[,] auxMatrix = new byte[xSize, ySize];

        for (int x = kernelSize / 2; x < xSize - kernelSize / 2; x++)
        {
            for (int y = kernelSize / 2; y < ySize - kernelSize / 2; y++)
            {
                byte nr = 0;
                for (int xK = -kernelSize / 2; xK <= kernelSize / 2; xK++)
                {
                    for (int yK = -kernelSize / 2; yK <= kernelSize / 2; yK++)
                    {
                        nr += binaryMatrix[x + xK, y + yK];
                    }
                }
                if (nr == kernelSize * kernelSize) auxMatrix[x, y] = 1;
                else auxMatrix[x, y] = 0;
            }
        }
        
        return auxMatrix;
    }
    public static byte[,] ApplyDilation(byte[,] binaryMatrix,byte kernelSize)
    {
        int xSize = binaryMatrix.GetLength(0);
        int ySize = binaryMatrix.GetLength(1);
        byte[,] auxMatrix = new byte[xSize, ySize];
        for (int x = kernelSize / 2; x < xSize - kernelSize / 2; x++)
        {
            for (int y = kernelSize / 2; y < ySize - kernelSize / 2; y++)
            {
                byte nr = 0;
                for (int xK = -kernelSize / 2; xK <= kernelSize / 2; xK++)
                {
                    for (int yK = -kernelSize / 2; yK <= kernelSize / 2; yK++)
                    {
                        nr += binaryMatrix[x + xK, y + yK];
                    }
                }
                if (nr > 0) auxMatrix[x, y] = 1;
                else auxMatrix[x, y] = 0;
            }
        }
        return auxMatrix;
    }

    public static byte[,] Difference(byte[,] m1, byte[,] m2)
    {
        if (m1.GetLength(0) != m2.GetLength(0) && m1.GetLength(1) != m2.GetLength(1))
        {
            return null;
        }
        else
        {
            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < m2.GetLength(1); j++)
                {
                    m1[i, j] -= m2[i, j];
                }
            }
            return m1;
        }
    }

    private static void GrassFire(int x, int y, byte blob, byte[,] matrix, byte[,] blobMatrix)
    {
        int xSize = blobMatrix.GetLength(0);
        int ySize = blobMatrix.GetLength(1);
        if (x > 0)
        {
            if (blobMatrix[x - 1, y] == 0 && matrix[x - 1, y] == 1)
            {
                blobMatrix[x - 1, y] = blob;
                GrassFire(x - 1, y, blob, matrix, blobMatrix);
            }
        }
        if (x < xSize - 1)
        {
            if (blobMatrix[x + 1, y] == 0 && matrix[x + 1, y] == 1)
            {
                blobMatrix[x + 1, y] = blob;
                GrassFire(x + 1, y, blob, matrix, blobMatrix);
            }
        }

        if (y > 0)
        {
            if (blobMatrix[x, y - 1] == 0 && matrix[x, y - 1] == 1)
            {
                blobMatrix[x, y - 1] = blob;
                GrassFire(x, y - 1, blob, matrix, blobMatrix);
            }
        }
        if (y < ySize - 1)
        {
            if (blobMatrix[x, y + 1] == 0 && matrix[x, y + 1] == 1)
            {
                blobMatrix[x, y + 1] = blob;
                GrassFire(x, y + 1, blob, matrix, blobMatrix);
            }
        }
    }

    public static byte[,] CalculateBlobs(byte[,] matrix, out byte numberOfBlobs)
    {
        int xSize = matrix.GetLength(0);
        int ySize = matrix.GetLength(1);
        byte[,] blobMatrix = new byte[xSize,ySize];
        byte blob = 1;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (blobMatrix[x, y] == 0 && matrix[x, y] == 1)
                {
                    blobMatrix[x, y] = blob;
                    GrassFire(x, y, blob, matrix, blobMatrix);
                    blob++;
                }
            }
        }
        numberOfBlobs = blob;
        
        return blobMatrix;
        
        
    }
    public static int CalculateArea(byte[,] blobMatrix,byte blob)
    {
        int area = 0;
        int xSize = blobMatrix.GetLength(0);
        int ySize = blobMatrix.GetLength(1);
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (blobMatrix[x, y] == blob)
                {
                    area++;
                }
            }
        }
        return area;
    }
    public static int CalculatePerimeter(byte[,] blobMatrix,byte blob)
    {
        int perimeter = 0;
        int xSize = blobMatrix.GetLength(0);
        int ySize = blobMatrix.GetLength(1);
        for (int x = 1; x < xSize - 1; x++)
        {
            for (int y = 1; y < ySize - 1; y++)
            {
                float avg = 0;
                for (int xk = -1; xk <= 1; xk++)
                {
                    for (int yk = -1; yk <= 1; yk++)
                    {
                        avg += blobMatrix[xk + x, yk + y];
                    }
                }
                avg /= 9.0f;
                if (Mathf.Abs(avg - blobMatrix[x, y]) > 0.01f)
                {
                    perimeter++;
                } 

            }
        }
        return perimeter;
    }

}
