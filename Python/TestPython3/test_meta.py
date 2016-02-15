
import unittest
import types

########################################################################
class MetaTest(unittest.TestCase):
    """demonstrate how to perform meta-programming in python"""

    def testType(self):
        """
        'is' can be used here, because during runtime, only one type object exists to represent a class of objects
        whether it is built-in type or user-defined type
        """
        int_type = type(5)
        self.assertTrue(int_type is int)
        
        type_type = type(int_type)
        self.assertTrue(type_type is type)
        
        self.assertTrue(type(3.14) is float)
        self.assertTrue(type(1234567891011121314151617181920) is int) # in Python 3, long has been merged into int
        
    def testIsinstance(self):
        """
        this testcase shows that 'isinstance' can be passed multiple target types
        to achieve a OR effect
        """
        self.assertTrue(isinstance(5,(int,float)))
        
        self.assertTrue(isinstance(3.14,(int,float)))
        self.assertFalse(isinstance(3.14,(int)))
        self.assertTrue(isinstance(3.14,(float)))
        
        self.assertFalse(isinstance("cheka",(int,float)))
        
    def testTypeName(self):
        for type_name in ["int","float","str","complex"]:
            type_obj = eval(type_name)
            self.assertTrue(isinstance(type_obj,type))
            self.assertEqual(type_name,type_obj.__name__)
        
if __name__ == "__main__":
    unittest.main()
        
        
    
    