
import unittest
        
class DynamicClassTest(unittest.TestCase):
    """like Reflection in other languages, access properties of an object by name"""
    class Helper(object):
        def __init__(self,initvalue = 10):
            self.member = initvalue
        
    def testHasAttr(self):
        obj = self.__class__.Helper()
        self.assertTrue(hasattr(obj,"member"))
        self.assertFalse(hasattr(obj,"notexisted"))
        
    def testGetAttr(self):
        obj = self.__class__.Helper(100)
        self.assertEqual(100,getattr(obj,"member"))
        
        obj.member = "cheka"
        self.assertEqual("cheka",getattr(obj,"member"))
        
        self.assertRaises(AttributeError,lambda:  getattr(obj,"noexisted"))
        
    def testSetAttr(self):
        # --------------- if the attribute exists, just replace old value with new value
        obj = self.__class__.Helper()
        setattr(obj,"member","newvalue")
        self.assertEqual("newvalue",obj.member)
        
        # --------------- otherwise, create a new attribute
        self.assertFalse(hasattr(obj,"newMember"))
        setattr(obj,"newMember",99)
        self.assertEqual(99,obj.newMember)
        
    def testVars(self):
        obj = self.__class__.Helper()
        setattr(obj,"newMember",99)
        self.assertEqual({"newMember":99,"member":10},vars(obj))
        
if __name__ == "__main__":
    unittest.main()