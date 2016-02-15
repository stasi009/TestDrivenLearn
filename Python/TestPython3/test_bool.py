
import unittest

########################################################################
class BoolTest(unittest.TestCase):
    """test how boolean values are used in python"""    
    
    def testConjunctionOperators(self):
        self.assertTrue( (2 < 4) and (5 > 1) )
        self.assertTrue( 2 < 3 < 4 )# a simple way to write "2 < 3 and 3 < 4"
        
    def testEquivalentTrue(self):
        self.assertTrue(1)
        self.assertTrue(-1) # any non-zero value is viewed as true, even negative numbers
        self.assertTrue([0]) # non-empty list can be viewed as true
            
    def testEquivalentFalse(self):
        """
        all empty containers and number zero are false
        """
        self.assertFalse(bool([]))
        self.assertFalse(bool(()))
        self.assertFalse(bool({}))
        self.assertFalse(0)
        self.assertFalse(None)
        
if __name__ == "__main__":
    unittest.main()
        
    
    