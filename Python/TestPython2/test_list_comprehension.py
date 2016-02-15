
import unittest

class ListComprehensionTest(unittest.TestCase):
    """
    using list comprehension can do the same work as map, filter
    but in a much simplified manner
    """
    
    def testAsFilter(self):
        odd_list = [x for x in xrange(6) if x%2==1]
        self.assertEqual([1,3,5],odd_list)
        
        even_list = [x for x in xrange(6) if x%2==0]
        self.assertEqual([0,2,4],even_list)
        
    def testAsMap(self):
        self.assertEqual([0,1,4,9],[x*x for x in xrange(4)])
        self.assertEqual([1,2,3,4],[x+1 for x in xrange(4)])
        
    def testMultiIterVariables(self):
        self.assertEqual([(0,0),(0,1),
                          (1,0),(1,1),
                          (2,0),(2,1)],[(x,y) for x in xrange(3) for y in xrange(2)])
        
        
        self.assertEqual(['cp', 'co', 'ct', 'ap', 'ao', 'at', 'tp', 'to', 'tt'],
                         [x+y for x in 'cat' for y in 'pot'])
        self.assertEqual(['cp', 'ct', 'ap', 'at'],
                         [x+y for x in 'cat' for y in 'pot' if x != 't' and y != 'o' ])
        self.assertEqual(['cp', 'co', 'ct', 'ap', 'ao', 'at', 'tp', 'tt'],
                         [x+y for x in 'cat' for y in 'pot' if x != 't' or y != 'o' ])
        
    def testSelectMany(self):
        """
        using list comprehension to simulate "SelectMany" feature in LINQ
        """
        full_names = ["Anne Williams","John Fred Smith"]
        expected_part_names = ["Anne","Williams","John","Fred","Smith"]
        actual_part_names = [partname for fullname in full_names for partname in fullname.split()]
        self.assertEqual(expected_part_names,actual_part_names)
        

if __name__ == "__main__":
    unittest.main()