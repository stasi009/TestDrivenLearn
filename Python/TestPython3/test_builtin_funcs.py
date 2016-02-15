
import unittest

########################################################################
class BuiltinFuncTest(unittest.TestCase):
    """demonstrate how to use built-in functions"""

    def testSorted(self):
        names = ("cheka","Stasi","mss")
        # sorted return a wholely new list, the input array will remainn not changed
        # the input is a sequence, which can be iterated
        # no matter what type the input is, the result is always a list
        self.assertEqual(["Stasi","cheka","mss"],sorted(names))
        self.assertCountEqual(["cheka","Stasi","mss"],names)
        
        # sorted is different from list.sort
        # sorted return a new list, while list.sort happen in place
        numbers = [3,2,1]
        self.assertEqual([1,2,3],sorted(numbers))
        self.assertEqual([3,2,1],numbers) # after sorted, the original list remain the same
        
        numbers.sort()
        self.assertEqual([1,2,3],numbers) # list.sort happen in place and modify the original input
        
    def testReversed(self):
        names = ("cheka","stasi","mss")
        
        # reversed return back an iterator, not a sequence
        reversed_names = []
        for item in reversed(names):
            reversed_names.append(item)
            
        self.assertEqual(["mss","stasi","cheka"],reversed_names)    
        
    def testEnumerate(self):
        name = "cheka"
        expected = ["c","h","e","k","a"]
        
        for index,ch in enumerate(name):
            self.assertEqual(expected[index],ch)    
            
        ### another test
        self.assertEqual([(0,"o"),(1,"k")],[e for e in enumerate("ok")])
            
    def testRange(self):
        self.assertSequenceEqual([0,1,2],range(3))
        self.assertSequenceEqual([1,2,3],range(1,4))#include the start, exclude the end
        self.assertSequenceEqual([1, 4, 7, 10, 13],range(1, 13+1, 3))
        self.assertSequenceEqual([10, 9, 8, 7, 6, 5],range(10, 5-1, -1))
        
    def testZip(self):
        a = [1,2]
        b = ("stasi","cheka")
        c = [9,8]
        iterator = zip(a,b,c) # it returns an iterator, not a list
        
        self.assertEqual((1,"stasi",9),next(iterator))
        self.assertEqual((2,"cheka",8),next(iterator))
        with self.assertRaises(StopIteration):
            next(iterator)
            
if __name__ == "__main__":
    unittest.main()
        
        
    
    