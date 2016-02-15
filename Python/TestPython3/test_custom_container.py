
import unittest
from packages1.mylist import MyList

class CustomContainerTest(unittest.TestCase):
    
    def testTypeValidation(self):
        intlist = MyList(int)
        intlist.append(5)
        self.assertRaises(TypeError, lambda : intlist.append("cheka") )
        
    def testMethodDelegation(self):
        """by delegate "__getattr__" to inside object, APIs of builtin type is still valid"""
        intlist = MyList(str)
        intlist.append("cheka")
        intlist.append("stasi")
        intlist.append("kgb")
        
        self.assertEqual(0,intlist.index("cheka") )    
        self.assertEqual("stasi",intlist.pop(1) )
        
    def testLength(self):
        intlist = MyList(int)
        for index in range(3):
            intlist.append(index)
        self.assertEqual(3,len(intlist))
        
    def testToBoolean(self):
        """if __bool__ is not defined, __len__ is used to determine whether True or False"""
        alist = MyList(int)
        self.assertFalse(alist) # length == 0
        
        alist.append(0)
        self.assertTrue(alist) # non-empty