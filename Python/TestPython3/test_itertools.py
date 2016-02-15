
import unittest
import itertools

class ItertoolsTest(unittest.TestCase):
    
    def testGroupby1(self):
        """
        before you use this stupid function
        you have to sort it with the same key function
        """
        a = [3,1,2,1,3]
        self.assertNotEqual(3,len(list(itertools.groupby(a))))
        
        b = sorted(a)
        self.assertEqual(3,len(list(itertools.groupby(b))))
        
        
    def testFlatMap(self):
        
        def flatmap(mapper,items):
            return itertools.chain.from_iterable( map(mapper,items) )
            
        actual = list(flatmap(lambda x: x,["abc","xy","z"]))
        self.assertEqual(["a","b","c","x","y","z"],actual)