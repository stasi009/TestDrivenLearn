
import unittest

########################################################################

def global_throw_exception(x,y):
    if x != y:
        raise ValueError

class PyUnitTest(unittest.TestCase):
    """
    demonstrate the usage of testing API in the PyUnit
    """

    def throwException(self,flag):
        if flag == 0:
            raise ValueError
        
        
    def testAssertEqual(self):
        self.assertEqual(10,10,"two integers are equal")
        self.assertEqual("stasi","stasi","two strings are equal")
        
        list1 = [1,"stasi",99]
        list2 = [1,"stasi",99]
        self.assertEqual(list1,list2,"two lists are equal")
        
    def testAlmostEqual(self):
        value1 = 9.0
        value2 = 9.0000000001
        value3 = 9.0001
        
        # default precision
        self.assertNotEqual(value1,value2)
        self.assertAlmostEqual(value1,value2)
        
        # user-defined precision
        self.assertNotAlmostEqual(value1,value3)
        self.assertAlmostEqual(value1,value3,3)
        
        self.assertAlmostEqual(value1,value3,delta=0.001)
        self.assertNotAlmostEqual(value1,value3,delta=0.00001)
        
        
    def testIsInstance(self):
        self.assertIsInstance(1,int)
        self.assertIsInstance(3.14,float)
        self.assertIsInstance("stasi",str)
        
    def testIn(self):
        list1 = [1,"stasi"]
        self.assertIn(1,list1)
        self.assertIn("stasi",list1)
        self.assertNotIn(100,list1)
        
    def testNone(self):
        self.assertIsNone(None)
        self.assertIsNotNone(9)
        
    def testIs(self):
        orilist = [1,2,3]
        cpylist = orilist[:] # copy
        
        ### equal but not the same object
        self.assertEqual(orilist,cpylist)
        self.assertIsNot(orilist,cpylist)
        self.assertIs(orilist,orilist)
        
    def testRaise(self):
        self.assertRaises(ValueError,self.throwException,0)
        self.assertRaises(ValueError,global_throw_exception,8,9)
        
    # assertRaise return a context manager so that 
    # the code under test can be written inline rather than as a function
    def testRaiseWith(self):
        # you must use keyword parameters, otherwise, it will treat the second parameter
        # as a callable, which will cause error
        with self.assertRaises(ValueError,msg="hit message here"):
            self.throwException(0)
            
        # 'cm' stands for context manager
        with self.assertRaises(ValueError) as cm:
            global_throw_exception(9,8)
            
    def testCountEqual(self):
        list1 = [1,2,3]
        list2 = [3,2,1] # same elements, just in different order
        
        self.assertNotEqual(list1,list2)
        self.assertCountEqual(list1,list2)
        
    def testSequenceEqual(self):
        """
        !!! only "list, tuple, range, string, bytes" are considered as sequence
        !!! generator, iterator is not sequence, so cannot be used by this method
        
        test the equality of all sequence types in a generic way
        same content but different types is not considered as equal
        but will be considered as 'sequence equal'
        
        !!! however, it cannot be used to test on a generator
        !!! because, generators don't have the 'length'
        !!! in general, it cannot be used on lazy collection, can only work on active ones
        """
        numlist = [1,2,3]
        numtuple = (1,2,3)
        self.assertNotEqual(numlist,numtuple)
        self.assertSequenceEqual(numtuple,numlist)
        
        numrange = range(1,4) # [1,4), range have the length and can be indexed
        self.assertNotEqual(numlist,numrange)
        self.assertSequenceEqual(numlist,numrange)
        
        charlist = ["s","t","a","s","i"]
        charstr = "stasi" 
        self.assertNotEqual(charlist,charstr)
        self.assertSequenceEqual(charstr,charlist)
        
if __name__ == "__main__":
    unittest.main()
        
        
    
    