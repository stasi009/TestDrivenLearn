
using System;
using System.Numerics;

using NUnit.Framework;

namespace CSharpBasicTest.Net4
{
    [TestFixture]
    sealed class ComplexTest
    {
        private Random m_rand;

        [SetUp]
        public void Setup()
        {
            m_rand = new Random();
        }

        double GetRandValue()
        {
            return m_rand.NextDouble() * 1000;
        }

        [Test]
        public void TestRectangleCreation()
        {
            double real = GetRandValue();
            double imaginary = GetRandValue();
            Complex complex = new Complex(real, imaginary);

            Assert.AreEqual(real, complex.Real, 1e-6);
            Assert.AreEqual(imaginary, complex.Imaginary, 1e-6);

            Assert.AreEqual(Math.Sqrt(real * real + imaginary * imaginary), complex.Magnitude, 1e-6);
            Assert.AreEqual(Math.Atan(imaginary / real), complex.Phase, 1e-6);
        }

        [Test]
        public void TestPolarCreation()
        {
            double real = GetRandValue();
            double imaginary = GetRandValue();

            double magnitude = Math.Sqrt(real * real + imaginary * imaginary);
            double phase = Math.Atan(imaginary / real);

            Complex complex = Complex.FromPolarCoordinates(magnitude, phase);

            Assert.AreEqual(real, complex.Real, 1e-6);
            Assert.AreEqual(imaginary, complex.Imaginary, 1e-6);
        }

        [Test]
        public void TestAdd()
        {
            Complex complex1 = new Complex(GetRandValue(), GetRandValue());
            Complex complex2 = new Complex(GetRandValue(), GetRandValue());

            Complex sum = complex1 + complex2;

            Assert.AreEqual(complex1.Real + complex2.Real, sum.Real, 1e-6);
            Assert.AreEqual(complex1.Imaginary + complex2.Imaginary, sum.Imaginary, 1e-6);
        }
    }
}