﻿using System;
using RayTracer.Utility;

namespace RayTracer.Model
{
    public class Matrix
    {
        public Matrix(int size)
        {
            Size = size;
            _matrix = new float[size * size];
        }

        public int Size { get; init; }

        public float Get(int row, int column)
        {
            return _matrix[(row * Size) + column];
        }

        public void Set(float value, int row, int column)
        {
            _matrix[(row * Size) + column] = value;
        }

        public bool IsEqual(Matrix other)
        {
            if (Size != other.Size)
            {
                return false;
            }

            for (int i = 0; i < Size * Size; i++)
            {
                if (!Numeric.FloatIsEqual(_matrix[i], other._matrix[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public Matrix Multiply(Matrix matrixTwo)
        {
            if (Size != matrixTwo.Size)
            {
                throw new NotSupportedException(
                    "Matrices of different sizes cannot be multiplied together");
            }

            var matrix = new Matrix(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var row = GetRow(i);
                    var column = matrixTwo.GetColumn(j);

                    var product = 0F;
                    for (int k = 0; k < Size; k++)
                    {
                        product += row[k] * column[k];
                    }

                    matrix.Set(product, i, j);
                }
            }

            return matrix;
        }

        public RayTuple Multiply(RayTuple tuple)
        {
            if (Size != 4)
            {
                throw new NotSupportedException(
                    "Only matrices of Size 4 can be multiplied by tuples");
            }

            var tupleValues = new float[4] {
                tuple.X,
                tuple.Y,
                tuple.Z,
                tuple.W
            };

            var newTupleValue = new float[4];
            for (int i = 0; i < tupleValues.Length; i++)
            {
                var row = GetRow(i);

                var product = 0F;
                for (int j = 0; j < Size; j++)
                {
                    product += row[j] * tupleValues[j];
                }

                newTupleValue[i] = product;
            }

            return new RayTuple(
                newTupleValue[0],
                newTupleValue[1],
                newTupleValue[2],
                newTupleValue[3]);
        }

        public Matrix Transpose()
        {
            var matrix = new Matrix(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    matrix.Set(Get(i, j), j, i);
                }
            }

            return matrix;
        }

        public float Determinant()
        {
            if (Size == 2)
            {
                return (Get(0, 0) * Get(1, 1)) - (Get(1, 0) * Get(0, 1));
            }
            else if (Numeric.IsWithinRange(2, 4, Size))
            {
                var determinant = 0F;

                for (var i = 0; i < Size; i++)
                {
                    determinant += Get(0, i) * Cofactor(0, i);
                }

                return determinant;
            }

            throw new NotSupportedException(
                    "Determinant can only be calculated on 2x2, 3x3 or 4x4 Matrices");
        }

        public Matrix SubMatrix(int rowIndexToDelete, int columnIndexToDelete)
        {
            if (!Numeric.IsWithinRange(0, Size - 1, rowIndexToDelete) ||
                !Numeric.IsWithinRange(0, Size - 1, columnIndexToDelete))
            {
                throw new IndexOutOfRangeException();
            }

            var subMatrix = new Matrix(Size - 1);
            var (subMatrixX, subMatrixY) = (0, 0);

            for (int i = 0; i < Size; i++)
            {
                if (i == rowIndexToDelete)
                {
                    continue;
                }

                for (int j = 0; j < Size; j++)
                {
                    if (j == columnIndexToDelete)
                    {
                        continue;
                    }

                    subMatrix.Set(
                        Get(i, j),
                        subMatrixX,
                        subMatrixY);

                    subMatrixY += 1;
                }

                subMatrixY = 0;
                subMatrixX += 1;
            }

            return subMatrix;
        }

        public float Minor(int rowToDelete, int columnToDelete)
        {
            return SubMatrix(rowToDelete, columnToDelete)
                .Determinant();
        }

        public float Cofactor(int row, int column)
        {
            var minor = Minor(row, column);

            if ((row + column) % 2 == 0)
            {
                return minor;
            }

            return minor * -1;
        }

        public Matrix Inverse()
        {
            var determinant = Determinant();

            if (determinant == 0)
            {
                throw new NotSupportedException(
                    "Inverse cannot be calculated on Matrices where determinant is zero");
            }

            var matrix = new Matrix(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var cofactor = Cofactor(i, j);

                    matrix.Set(cofactor / determinant, j, i);
                }
            }

            return matrix;
        }

        private float[] GetRow(int index)
        {
            var row = new float[Size];

            for (int i = 0; i < Size; i++)
            {
                row[i] = Get(index, i);
            }

            return row;
        }

        private float[] GetColumn(int index)
        {
            var column = new float[Size];

            for (int i = 0; i < Size; i++)
            {
                column[i] = Get(i, index);
            }

            return column;
        }

        private readonly float[] _matrix;
    }
}
