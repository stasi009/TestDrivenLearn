
import unittest
import math
import cmath # math for complex numbers
import random

class ComplexTest(unittest.TestCase):
    def setUp(self):
        self._real = random.uniform(1,100) 
        self._imag = random.uniform(1,100) 
        self._cplx = complex(self._real,self._imag)
        
    def testHardcoded(self):
        # manually create the complex number
        cv = 1+1j # 1+j is invalid, since j will be regarded as one variable
        self.assertAlmostEqual(math.sqrt(2),abs(cv))
        
        cv = 2+3j
        self.assertEqual(3,cv.imag)
        
    def testRealImaginary(self):
        self.assertAlmostEqual(self._real,self._cplx.real)
        self.assertAlmostEqual(self._imag,self._cplx.imag)
    
    def testAbs(self):
        expected_abs = math.sqrt(self._real ** 2 + self._imag ** 2)
        self.assertAlmostEqual(expected_abs,abs(self._cplx))
        
    def testEqual(self):
        self.assertEqual(complex(self._real),self._real)
        
        a = 1 + 1j
        b = 1 + 1j
        self.assertEqual(a,b)
        self.assertTrue(a==b)
        
    def testPhase(self):
        val = random.uniform(1,100)
        self.assertAlmostEqual(math.pi/4,cmath.phase( complex(val,val) ))
        
    def testPolar(self):
        """
        cmath.polar return (magnitude, phasor) of a complex number
        """
        magnitude,angle = cmath.polar(self._cplx)
        self.assertAlmostEqual(magnitude,abs(self._cplx))
        self.assertAlmostEqual(angle,cmath.phase(self._cplx))
        
    def testRect(self):
        mag = random.uniform(1,100)
        ang = random.uniform(1,100) # angle can exeeds the rrange of [-pi,pi]
        cpx = cmath.rect(mag,ang)
        
        self.assertAlmostEqual(mag * math.cos(ang),cpx.real)
        self.assertAlmostEqual(mag * math.sin(ang),cpx.imag)
        
    def testType(self):
        cv = 2 + 3j
        self.assertIsInstance(cv,complex)
        self.assertTrue(isinstance(cv,complex))
        self.assertTrue(type(cv) is complex)
        
if __name__ == "__main__":
    unittest.main()