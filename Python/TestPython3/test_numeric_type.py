
import unittest

class NumericTypeTest(unittest.TestCase):
    
    def testIsInstance(self):
        self.assertIsInstance(1,int)
        # in python 3, there is only one "integer" type, that is int
        # no long type any more
        self.assertIsInstance(1234567891011121314151617181920,int)
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
        
if __name__ == "__main__":
    unittest.main()