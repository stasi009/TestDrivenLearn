
import unittest

class TupleTest(unittest.TestCase):
    def testConstructFromIterable(self):
        atuple = tuple("cheka")
        self.assertEqual(("c","h","e","k","a"),atuple)
        
        # --------- the argument must be iterable
        self.assertRaises( TypeError, tuple,2 )
        
        # --------- just iterate the parameter and put each element into tuple
        # --------- so for dictionary, just put key into tuple
        adict = {1:"cheka","stasi":2}
        self.assertCountEqual((1,"stasi"),tuple(adict))# cannot guarantee the order
        
    def testConstructFromSingle(self):
        """put a comma, to create a tuple a single item"""
        number = 100
        
        # ------------------------ still original type without comma
        still_number = (number) # here () not the constructor of tuple, but common,ordinary math operator
        self.assertTrue(type(still_number) is int)
        
        # ------------------------ convert to tuple with comma added
        single_item_tuple = (number,)
        self.assertTrue(type(single_item_tuple) is tuple)
        self.assertEqual(1,len(single_item_tuple))
        self.assertEqual(number,single_item_tuple[0])
        
    def testPackUnpack(self):
        atuple = 1,"stasi",99.9
        personId,name,score = atuple
        self.assertEqual(1,personId)
        self.assertEqual("stasi",name)
        self.assertAlmostEqual(99.9,score)
        
    def testConcatenate(self):
        tuple1 = ("cheka","stasi")
        tuple2 = (1,2)
        self.assertEqual(("cheka","stasi",1,2),tuple1 + tuple2)
        
    def testEquality(self):
        # with or without (), both ways work
        tuple1 = "cheka",1
        tuple2 = ("cheka",1)
        
        self.assertEqual(tuple1,tuple2)
        self.assertTrue(tuple1 == tuple2)
        
        self.assertTrue(tuple1 is not tuple2)
        self.assertNotEqual(id(tuple1),id(tuple2))
        
        # ------- but "tuple" a tuple, will not create a new instance
        same_tuple = tuple(tuple1)
        self.assertTrue(same_tuple is tuple1);
        
    def testSlice(self):
        """pay attention that a slice of tuple will return another tuple, not list"""
        atuple = ("cheka",1,100.0)
        self.assertEqual(("cheka",1),atuple[:-1])
        self.assertNotEqual(["cheka",1],atuple[:-1])
        
    def testIndex(self):
        atuple = (1,"stasi",99.9)
        self.assertEqual(1,atuple[0])
        self.assertEqual("stasi",atuple[1])
        
if __name__ == "__main__":
    unittest.main()
