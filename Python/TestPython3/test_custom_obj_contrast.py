
import unittest

class CustomObjContrastTest(unittest.TestCase):
    """
    check that if custom object implement some special methods
    their behaviours are changed in the containers
    """
    class NoSpecialMethodObj(object):
        def __init__(self,identifier):
            self._identifier = identifier
            
    class WithSpecialMethodObj(NoSpecialMethodObj):
        
        def __init__(self,identifier):
            CustomObjContrastTest.NoSpecialMethodObj.__init__(self,identifier)
        
        def __eq__(self,rhs):
            if self is rhs:
                return True
            elif isinstance(rhs,CustomObjContrastTest.NoSpecialMethodObj):
                return self._identifier == rhs._identifier
            else:
                return False
            
        def __ne__(self,rhs):
            return not (self == rhs)
        
    # =================================================== #
    def testIn_NoSpecialMethod(self):
        """
        if not override "__eq__", "in" can only find same object, other than different object with same content
        """
        obj1 = CustomObjContrastTest.NoSpecialMethodObj(1)
        alist = [obj1]
        self.assertTrue(obj1 in alist)
        
        obj2 = CustomObjContrastTest.NoSpecialMethodObj(1)
        self.assertTrue(obj2 not in alist)
        
    def testIn_WithSpecialMethod(self):
        """
        if override "__eq__", "in" can find different object but with same content
        """
        alist = [CustomObjContrastTest.WithSpecialMethodObj(100)]
        obj2 = CustomObjContrastTest.WithSpecialMethodObj(100)
        self.assertTrue(obj2 in alist)
    
        
    def testCollectionEqual_NoSpecialMethod(self):
        """
        check equality of collection based on reference equality of each item
        """
        list1 = [CustomObjContrastTest.NoSpecialMethodObj(1)]
        list2 = [CustomObjContrastTest.NoSpecialMethodObj(1)]
        self.assertNotEqual(list1,list2)
        
    def testCollectionEqual_WithSpecialMethod(self):
        """
        since item's __eq__ has been overriden to check their content other than refernce
        then collection can also automatically check equality based on content other than reference
        """
        list1 = [CustomObjContrastTest.WithSpecialMethodObj(1)]
        list2 = [CustomObjContrastTest.WithSpecialMethodObj(1)]
        
        self.assertTrue(list1 is not list2)
        self.assertTrue(list1[0] is not list2[0])
        self.assertEqual(list1,list2)
        
if __name__ == "__main__":
    unittest.main()