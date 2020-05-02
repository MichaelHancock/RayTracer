﻿using System;
using AutoFixture;
using RayTracer.Model;
using Xunit;

namespace RayTracer.UnitTests
{
    public class RayTupleTests
    {
        private readonly Fixture _fixture;

        public RayTupleTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void RayTuple_With_W_ValueOfZeroIsAVector()
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);
            var w = 0;

            // Act
            var tuple = new RayTuple(x, y, z, (RayTupleType)w);

            // Assert
            Assert.Equal(RayTupleType.Vector, tuple.W);
        }

        [Fact]
        public void RayTuple_With_W_ValueOfOneIsAPoint()
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);
            var w = 1;

            // Act
            var tuple = new RayTuple(x, y, z, (RayTupleType)w);

            // Assert
            Assert.Equal(RayTupleType.Point, tuple.W);
        }

        [Fact]
        public void RayVector_CreatesBaseTupleWith_W_ValueOfZero()
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);

            // Act
            var vector = new RayVector(x, y, z);

            // Assert
            Assert.Equal((RayTupleType)0, vector.W);
        }

        [Fact]
        public void RayPoint_CreatesBaseTupleWith_W_ValueOfOne()
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);

            // Act
            var point = new RayPoint(x, y, z);

            // Assert
            Assert.Equal((RayTupleType)1, point.W);
        }

        [Fact]
        public void RayTuple_IsEqual_ReturnsTrueIfTuplesAreEqual()
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);

            var tupleOne = new RayVector(x, y, z);
            var tupleTwo = new RayVector(x, y, z);

            // Act
            var isEqual = tupleOne.IsEqual(tupleTwo);
            
            // Assert
            Assert.True(isEqual);
        }

        [Fact]
        public void RayTuple_IsEqual_ReturnsTrueIfTuplePositionValuesAreDifferentByLessThanFiveDP()
        {
            // Arrange
            var (x, y, _) = CreateRandomPosition(_fixture);
            var zOne = 1.23456F;
            var zTwo = zOne + 0.000005F;

            var tupleOne = new RayVector(x, y, zOne);
            var tupleTwo = new RayVector(x, y, zTwo);

            // Act
            var isEqual = tupleOne.IsEqual(tupleTwo);

            // Assert
            Assert.True(isEqual);
        }

        [Fact]
        public void RayTuple_IsEqual_ReturnsFalseIfTuplesPositionValuesAreDifferent()
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);
            var (xTwo, yTwo, zTwo) = CreateRandomPosition(_fixture);

            var tupleOne = new RayPoint(xOne, yOne, zOne);
            var tupleTwo = new RayPoint(xTwo, yTwo, zTwo);

            // Act
            var isEqual = tupleOne.IsEqual(tupleTwo);

            // Assert
            Assert.False(isEqual);
        }

        [Fact]
        public void RayTuple_IsEqual_ReturnsFalseIfTuples_W_ValuesAreDifferent()
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);

            var tupleOne = new RayPoint(x, y, z);
            var tupleTwo = new RayVector(x, y, z);

            // Act
            var isEqual = tupleOne.IsEqual(tupleTwo);

            // Assert
            Assert.False(isEqual);
        }

        [Fact]
        public void RayTuple_Add_ReturnsANewTupleWithPropertiesSummedTogether()
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);
            var (xTwo, yTwo, zTwo) = CreateRandomPosition(_fixture);

            var tupleOne = new RayPoint(xOne, yOne, zOne);
            var tupleTwo = new RayVector(xTwo, yTwo, zTwo);

            // Act
            var summedTuple = tupleOne.Add(tupleTwo);

            // Assert
            Assert.Equal(xOne + xTwo, summedTuple.X);
            Assert.Equal(yOne + yTwo, summedTuple.Y);
            Assert.Equal(zOne + zTwo, summedTuple.Z);
            Assert.Equal((RayTupleType)((int)tupleOne.W + (int)tupleTwo.W), summedTuple.W);
        }

        [Theory]
        [InlineData(RayTupleType.Vector, RayTupleType.Vector, typeof(RayVector))]
        [InlineData(RayTupleType.Vector, RayTupleType.Point, typeof(RayPoint))]
        public void RayTuple_Add_ReturnsTheCorrectType(RayTupleType wOne, RayTupleType wTwo, Type type)
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);
            var (xTwo, yTwo, zTwo) = CreateRandomPosition(_fixture);

            var tupleOne = new RayTuple(xOne, yOne, zOne, wOne);
            var tupleTwo = new RayTuple(xTwo, yTwo, zTwo, wTwo);

            // Act
            var summedTuple = tupleOne.Add(tupleTwo);

            // Assert
            Assert.IsType(type, summedTuple);
        }

        [Fact]
        public void RayTuple_Subtract_ReturnsANewTupleWithPropertiesSubtracted()
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);
            var (xTwo, yTwo, zTwo) = CreateRandomPosition(_fixture);

            var tupleOne = new RayPoint(xOne, yOne, zOne);
            var tupleTwo = new RayVector(xTwo, yTwo, zTwo);

            // Act
            var subtractedTuple = tupleOne.Subtract(tupleTwo);

            // Assert
            Assert.Equal(xOne - xTwo, subtractedTuple.X);
            Assert.Equal(yOne - yTwo, subtractedTuple.Y);
            Assert.Equal(zOne - zTwo, subtractedTuple.Z);
            Assert.Equal((RayTupleType)(tupleOne.W - tupleTwo.W), subtractedTuple.W);
        }

        [Theory]
        [InlineData(RayTupleType.Vector, RayTupleType.Vector, typeof(RayVector))]
        [InlineData(RayTupleType.Point, RayTupleType.Vector, typeof(RayPoint))]
        public void RayTuple_Subtract_ReturnsTheCorrectType(RayTupleType wOne, RayTupleType wTwo, Type type)
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);
            var (xTwo, yTwo, zTwo) = CreateRandomPosition(_fixture);

            var tupleOne = new RayTuple(xOne, yOne, zOne, wOne);
            var tupleTwo = new RayTuple(xTwo, yTwo, zTwo, wTwo);

            // Act
            var summedTuple = tupleOne.Subtract(tupleTwo);

            // Assert
            Assert.IsType(type, summedTuple);
        }

        [Theory]
        [InlineData(RayTupleType.Point)]
        [InlineData(RayTupleType.Vector)]
        public void RayTuple_Negate_ReturnsANewNegatedTuple(RayTupleType w)
        {
            // Arrange
            var (x, y, z) = CreateRandomPosition(_fixture);
            var tuple = new RayTuple(x, y, z, w);

            // Act
            var negatedTuple = tuple.Negate();

            // Assert
            Assert.Equal(x * -1, negatedTuple.X);
            Assert.Equal(y * -1, negatedTuple.Y);
            Assert.Equal(z * -1, negatedTuple.Z);
            Assert.Equal((RayTupleType)((int)w * -1), negatedTuple.W);
        }

        [Fact]
        public void RayTuple_Multiply_ReturnsANewTupleWithPropertiesMultipliedByScaler()
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);

            var tuple = new RayPoint(xOne, yOne, zOne);

            // Act
            var multipliedTuple = tuple.Multiply(1.5F);

            // Assert
            Assert.Equal(xOne * 1.5F, multipliedTuple.X);
            Assert.Equal(yOne * 1.5F, multipliedTuple.Y);
            Assert.Equal(zOne * 1.5F, multipliedTuple.Z);
            Assert.Equal((RayTupleType)((int)tuple.W * 1.5F), multipliedTuple.W);
        }

        [Theory]
        [InlineData((RayTupleType)4, 0.25F, typeof(RayPoint))]
        [InlineData(RayTupleType.Vector, 2F, typeof(RayVector))]
        public void RayTuple_Multiply_ReturnsTheCorrectType(RayTupleType w, float scaler, Type type)
        {
            // Arrange
            var (xOne, yOne, zOne) = CreateRandomPosition(_fixture);

            var tuple = new RayTuple(xOne, yOne, zOne, w);

            // Act
            var multipliedTuple = tuple.Multiply(scaler);

            // Assert
            Assert.IsType(type, multipliedTuple);
        }

        private (float x, float y, float z) CreateRandomPosition(Fixture fixture) =>
            (fixture.Create<float>(), fixture.Create<float>(), fixture.Create<float>());
    }
}
