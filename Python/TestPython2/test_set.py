
import unittest

class SetTest(unittest.TestCase):
    
    def testHybridTypes(self):
        aset = set([42, 'a string', (25, 4)])
        self.assertTrue((25,4) in aset)
    
    def testConstruct(self):
        aset = set("bookshop")
        self.assertItemsEqual(["b","h","k","o","p","s"],aset)
        
    def testAdd(self):
        aset = set("abc")
        ori_id = id(aset)
        
        # add duplicated element
        aset.add("a")
        # assertItemsEqual is renamed as "assertCountEqual"
        self.assertItemsEqual(["a","c","b"],aset)
        
        aset.add("z")
        now_id = id(aset)
        
        self.assertItemsEqual(["a","c","b","z"],aset)
        # shows that set is mutable, not copy-on-write
        self.assertEqual(ori_id,now_id)
        
    def testIn(self):
        aset = set("abc")
        self.assertTrue("a" in aset)
        self.assertTrue("z" not in aset)
        
    def testEqual(self):
        self.assertEqual(set("shop"),set("pohs"))
        
    def testUnion(self):
        set1 = set("cheka")
        set2 = set("abc")
        self.assertEqual(set("chekab"),set1 | set2)
        
    def testIntersection(self):
        set1 = set("cheka")
        set2 = set("abc")
        self.assertEqual(set("ac"),set1 & set2)
        self.assertEqual(set("ac"),set1.intersection(set2)) # different ways to invoke the same method
        
    def testContainOnlyImmutables(self):
        """
        A set can only contain hashable, immutable data types. 
        Integers, strings, and tuples are hashable; 
        lists, dictionaries, and other sets (except frozensets, see below) are not.
        """
        aset = set()
        
        aset.add(1)
        aset.add("cheka")
        
        # non-hashable object (that is mutable) objects cannot be contained in set
        self.assertRaises(TypeError, lambda : aset.add([]) )
        
if __name__ == "__main__":
    unittest.main()