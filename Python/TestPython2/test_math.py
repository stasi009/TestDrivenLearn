
import unittest
import random
import math

class MathTest(unittest.TestCase):

    def testDivZeroException(self):
        """
        different from other languanges, divide zero won't return Inf
        but it will throw an exception
        no matter it is a "integer division" or "float division"
        """
        with self.assertRaises(ZeroDivisionError):
            v = 1.0/0.0

        with self.assertRaises(ZeroDivisionError):
            v = 1/0

    def testPower(self):
        """two ways to do the power calculation
        the difference is that:
        ** can only deal with integer
        while math.pow can handle floating-point numbers"""
        self.assertEqual(9,3**2)
        self.assertEqual(8,2**3)
        self.assertIsInstance(3**2,int)
        
        self.assertAlmostEqual(9.0,math.pow(3,2))
        self.assertAlmostEqual(8.0,math.pow(2,3))
        self.assertIsInstance(math.pow(2,3),float)
    
    def testDivide(self):
        ################### INTEGER DIVISION
        self.assertEqual(1,3/2)
        self.assertEqual(0,3/4)
        self.assertEqual(1,4/3)

        ################### FLOAT DIVISION
        # to get "float division", you have to write the number like "x.0" or use float(x)
        self.assertAlmostEqual(1.5,3.0/2)
        self.assertAlmostEqual(0.75,3/4.0)
        self.assertAlmostEqual(1.333333333333333,float(4)/3)
        self.assertAlmostEqual(1.5,3.0/2)

        ################### MOD
        self.assertEqual(1,3%2)
        self.assertEqual(2,10%4)
        
        ################### FLOOR DIVISION
        # no matter what the type is, int or float
        # // always perform the floor division
        self.assertEqual(1,3//2)
        self.assertAlmostEqual(1.0,3.0//2.0)
    
    def testDivmod(self):
        x = random.randint(1,100)
        y = random.randint(1,100)
        
        quo,resid = divmod(x,y)
        
        self.assertEqual(x//y,quo)
        self.assertEqual(x%y,resid)
        
    def testFloorTruncRound(self):
        # ------------ floor always returns the next lower integer
        self.assertEqual(2,math.floor(2.9))
        self.assertEqual(-3,math.floor(-2.9))
        
        # ------------ trunc just truncates digital parts and remain the integer part
        self.assertEqual(2,math.trunc(2.9))
        self.assertEqual(-2,math.trunc(-2.9))
        
        # ------------ int just works like the trunc, just truncate other than round
        self.assertEqual(2,int(2.9))
        self.assertEqual(-2,int(-2.9))
        
        # ------------ round returns a float
        self.assertEqual(3,round(2.9))
        self.assertEqual(-3,round(-2.9))
        
        self.assertEqual(2,round(2.1))
        self.assertEqual(-2,round(-2.1))
        
        # ------------ round returns a float
        self.assertEqual(3,round(2.93))
        self.assertAlmostEqual(2.9,round(2.93,1))
        
    def testRound(self):
        v = 9.93
        
        a = round(v)
        self.assertAlmostEqual(10.0,a)
        self.assertIsInstance(a,float)
        self.assertTrue(isinstance(a,float))
        
        b = round(v,1)
        self.assertEqual(9.9,b)
        self.assertIsInstance(b,float)
        
if __name__ == "__main__":
    unittest.main()