
import unittest

class MutableImmutableTest(unittest.TestCase):
    def testIncrementImmutable(self):
        a = 1
        b = a
        oldid = id(a)
        
        a += 1 # silently create a new object, and point a to that new object
        newid = id(a)
        
        # a points to a new object
        # b still points to the old object
        self.assertEqual(1,b)
        self.assertNotEqual(oldid,newid)
        self.assertEqual(oldid,id(b))
        
    def testIncrementMutable(self):
        a = [0]
        b = a
        oldid = id(a)
        
        a += [1,2]
        newid = id(a)
        
        # a, b still points to the same old object
        self.assertEqual(oldid,newid)
        self.assertEqual([0,1,2],b)
        
    def testAnExample(self):
        def append_seq(oldseq):
            oldseq += (9,9)
            return oldseq
            
        oldlist = [1,2,3]
        oldtuple = (1,2,3)
        
        newlist = append_seq(oldlist)
        newtuple = append_seq(oldtuple)
        
        self.assertEqual([1,2,3,9,9],oldlist)
        self.assertEqual([1,2,3,9,9],newlist)
        self.assertTrue(newlist is oldlist)
        self.assertIs(oldlist,newlist)
        
        self.assertEqual((1,2,3),oldtuple)
        self.assertEqual((1,2,3,9,9),newtuple)