
import unittest
import math

class NumericTypeTest(unittest.TestCase):

    def testNan(self):
        v = float("nan")
        self.assertTrue(math.isnan(v))

    def test_inf(self):
        v = float("inf")
        self.assertTrue( math.isinf(v) )

    def testDivZeroNotReturnInf(self):
        """
        different from other languanges, divide zero won't return Inf
        but it will throw an exception
        no matter it is a "integer division" or "float division"
        """
        with self.assertRaises(ZeroDivisionError):
            v = 1.0/0.0

        with self.assertRaises(ZeroDivisionError):
            v = 1/0
    
    def testIsInstance(self):
        self.assertIsInstance(1,int)
        # in python 3, there is only one "integer" type, that is int, no long type any more
        # but in python 2, there is still "long" type
        self.assertIsInstance(1234567891011121314151617181920,long)
        # XXX self.assertIsInstance(100L,int) --- 'L' isn't necessary, and is invalid in Python 3
        self.assertIsInstance(3.14,float)
        
        self.assertTrue(isinstance(5,int))
        self.assertTrue(type(5) is int)
        
        self.assertTrue(isinstance(3.14,float))
        self.assertTrue(type(3.14) is float)
        
        self.assertTrue(isinstance(True,bool))    
        
    def testReference(self):
        a = 9
        b = a
        self.assertIs(a,b)
        
        a += 1
        self.assertIsNot(a,b)
        
    # for performance purpose
    # sometimes, python share value object among different variables
    def testReference2(self):
        a = 9
        b = 9
        self.assertIs(a,b)
        
        c = 10000000000000000000000000
        d = 10000000000000000000000000
        self.assertIs(c,d)
        
    def testFromToString(self):
        self.assertEqual(9,int("9"))
        self.assertRaises(ValueError,lambda : float("not number")) 
        self.assertEqual("9",str(9))
        self.assertEqual("3.14",str(3.14))   
        
    def test_cast(self):
        self.assertEqual(9,int(9.1))        
        self.assertEqual(6,int(6.9))
        self.assertAlmostEqual(1.0,float(1))          

    def test_round(self):
        self.assertAlmostEqual(1.0,round(1.4))
        self.assertAlmostEqual(2.0,round(1.5))
        self.assertAlmostEqual(-1.0,round(-1.4))
        self.assertAlmostEqual(-2.0,round(-1.5))
        
if __name__ == "__main__":
    unittest.main()