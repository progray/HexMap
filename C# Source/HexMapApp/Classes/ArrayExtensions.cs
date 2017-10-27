namespace Classes.ArrayExtensions
{
    public static class ArrayExtensions
    {
        // Изменение размерности двумерного массива
        public static T[,] ResizeArray<T>(this T[,] original, int width, int height)
        {
            T[,] newArray = new T[width, height];
            for (int x = 0; x < width && x <= original.GetUpperBound(0); x++)
                for (int y = 0; y < height && y <= original.GetUpperBound(1); y++)
                    newArray[x, y] = original[x, y];
            return newArray;
        }
    }
}
