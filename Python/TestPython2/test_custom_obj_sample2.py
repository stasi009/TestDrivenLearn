
import unittest
from packages1.mytime import MyTime

class CustomObjSample2Test(unittest.TestCase):
    def testStr(self):
        self.assertEqual("11:21",str(MyTime(11,21)))
        
    def testEqual(self):
        tm1 = MyTime(11,26)
        tm2 = MyTime(11,26)
        
        self.assertNotEqual(1,tm1) # compare different type
        
        self.assertTrue(tm1 is not tm2)
        self.assertEqual(tm1,tm2)
        self.assertTrue(tm1 == tm2)
        
    def testNotEqual(self):
        tm1 = MyTime(11,26)
        tm2 = MyTime(11,30)
        self.assertNotEqual(tm1,tm2)
        
    def testAdd(self):
        tm1 = MyTime(11,26)
        tm2 = MyTime(11,30)
        
        expected = MyTime(22,56)
        self.assertEqual(expected,tm1 + tm2)
        
        tm1 += tm2 # in place add
        self.assertEqual(expected,tm1)
        
    def testIterate(self):
        tm = MyTime(11,48)
        self.assertEqual([11,48],[num for num in tm])
        self.assertEqual([11,48],list(num for num in tm))
        
    def testBool(self):
        self.assertTrue(MyTime(1,0))
        self.assertFalse(MyTime(0,0))
        
    def testCompare(self):
        tm1 = MyTime(11,50)
        tm2 = MyTime(11,40)
        self.assertTrue(tm1 > tm2)
        
        tm1 = MyTime(11,50)
        tm2 = MyTime(12,5)
        self.assertTrue(tm1 < tm2)
        
    def testCustomGetAttr(self):
        tm1 = MyTime(11,50)
        self.assertEqual(0,tm1.second)
        self.assertRaises(AttributeError,lambda:  tm1.day) 
        
    def testGetItem(self):
        tm = MyTime(14,30)
        self.assertEqual(14,tm[0])
        self.assertEqual(30,tm[1])
        self.assertRaises(IndexError,lambda : tm[3])
        
    def testCallable(self):
        tm = MyTime(3,45)
        self.assertEqual((3,45),tm())
        
    def testHashable(self):
        schedule = {MyTime(1,20):"meeting",MyTime(5,30):"go back home"}
        self.assertIn(MyTime(5,30),schedule)
        self.assertEqual("meeting",schedule[MyTime(1,20)])
