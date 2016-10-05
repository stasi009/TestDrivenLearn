
import unittest
import collections

class DefaultDictTest(unittest.TestCase):

    def sample(self):
        d = defaultdict(lambda: [None,None])
        d["x"][0] = 1
        d["y"][1] = 99
        d['z'][1] = -1
        d['x'][1] = 8

    def _count_without_defdict(self,sequence):
        counts = {}
        for e in sequence:
            if e in counts:
                counts[e] += 1
            else:
                counts[e] = 1
        return counts

    # by using defaultdict, above method can be simplified
    def _count_with_defdict(self,sequence):
        counts = collections.defaultdict(int) # "int" is the factory, which will produce 0
        for e in sequence:
            counts[e] += 1
        return counts

    def test_count(self):
         s = 'mississippi'
         counts = self._count_with_defdict(s)
         # [('i', 4), ('p', 2), ('s', 4), ('m', 1)]
         self.assertEqual(4,counts['i'])
         self.assertEqual(2,counts['p'])
         self.assertEqual(4,counts['s'])
         self.assertEqual(1,counts['m'])

    def test_group(self):
        s = [('yellow', 1), ('blue', 2), ('yellow', 3), ('blue', 4), ('red', 1)]
        d = collections.defaultdict(list)# list is the factory, which will create an empty list
        for k,v in s:
            d[k].append(v)
        # [('blue', [2, 4]), ('red', [1]), ('yellow', [1, 3])]
        self.assertEqual([2,4],d["blue"])
        self.assertEqual([1],d["red"])
        self.assertEqual([1,3],d["yellow"])

    def test_initial_values(self):
        current = {"green": 12, "blue": 3}
        missing = [0]
        def count_missing_return_default():
            missing[0] += 1
            return 0

        counters = collections.defaultdict(count_missing_return_default,current)

        increments = [("red",5),("blue",17),("orange",9)]
        for k,v in increments:
            counters[k] += v

        # check the result
        self.assertEqual(2,missing[0])
        self.assertEqual({"green": 12, "blue": 20,"red":5,"orange":9},dict(counters))


class CounterTest(unittest.TestCase):

    def test_demo1(self):
        c = collections.Counter('gallahad')
        self.assertEqual(3,c['a'])
        self.assertEqual(2,c['l'])
        self.assertEqual(0,c['x'])# non-existed, return 0

class NamedTupleTest(unittest.TestCase):

    def test_demo1(self):
        Point = collections.namedtuple("Point",("x","y"))
        p = Point(1,2)
        self.assertEqual(1,p.x)
        self.assertEqual(2,p.y)

        # attributes are read-only, cannot be set
        with self.assertRaises(AttributeError):
            p.x = 3
