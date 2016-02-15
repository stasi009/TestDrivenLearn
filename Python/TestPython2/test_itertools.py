
import unittest
import itertools

class ItertoolsTest(unittest.TestCase):

    def test_imap1(self):
        it = itertools.imap(lambda x:x*x,xrange(3))
        self.assertEqual(0,next(it))
        self.assertEqual(1,next(it))
        self.assertEqual(4,next(it))
        with self.assertRaises(StopIteration):
            next(it)

    def test_imap2(self):
        it = itertools.imap(lambda x: x*x,(6,7,8))
        self.assertEqual(36,next(it))
        
        remainings = list(it)
        self.assertEqual([49,64],remainings)

    def testLazyEvaluatedGenerator(self):
        sideeffect = [0]
        def fool(x):
            sideeffect[0] += 1
            return x * x
        
        a = [1,2,3]
        gen = itertools.imap(fool,a)# just create a view, not evaluated yet
        
        self.assertEqual(0,sideeffect[0])
        
        list1 = list(gen)
        self.assertEqual([1,4,9],list1)
        self.assertEqual(3,sideeffect[0])
        
        # it is a generator, which can be executed only once
        list2 = list(gen)
        self.assertEqual(0,len(list2))
        self.assertEqual(3,sideeffect[0])

    def testIzip(self):
        a = [1,2]
        b = ("stasi","cheka")
        c = [9,8,101,102] # result length is determined by the shortest
        iterator = itertools.izip(a,b,c) # it returns an iterator, not a list
        
        self.assertEqual((1,"stasi",9),next(iterator))
        self.assertEqual((2,"cheka",8),next(iterator))
        with self.assertRaises(StopIteration):
            next(iterator)
    
    def testGroupby1(self):
        """
        before you use this stupid function
        you have to sort it with the same key function
        """
        a = [3,1,2,1,3]
        self.assertNotEqual(3,len(list(itertools.groupby(a))))
        
        b = sorted(a)
        self.assertEqual(3,len(list(itertools.groupby(b))))

    def testGroup2(self):
        a = [("stasi",2),("cheka",3),("cheka",25),
             ("gehlen",5),("stasi",3),("gehlen",2),
             ("stasi",4)]

        # you have to sort before groupby, using the same key function
        keyfunc = lambda x: x[0]
        aftersort = sorted(a,key=keyfunc)

        counts = {}
        for key,group in itertools.groupby(aftersort,keyfunc):
            counts[key] = reduce(lambda a,e: a+1,group,0)

        self.assertItemsEqual(("stasi","cheka","gehlen"),counts.keys())
        self.assertItemsEqual((3,2,2),counts.values())
        
    def test_islice(self):
        a = xrange(100)
        b = itertools.islice(a,1,4)
        self.assertEqual([1,2,3],list(b))
        self.assertEqual([],list(b))# b is a iterator, stateful, so cannot be iterated a second time
        
    def testFlatMap(self):
        
        def flatmap(mapper,items):
            return itertools.chain.from_iterable( map(mapper,items) )
            
        actual = list(flatmap(lambda x: x,["abc","xy","z"]))
        self.assertEqual(["a","b","c","x","y","z"],actual)