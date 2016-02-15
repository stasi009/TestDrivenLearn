
import unittest
import functools

class MapReduceFilterTest(unittest.TestCase):
    
    def testFilter(self):
        values = range(10)
        
        odds = list(filter(lambda x : x%2==1,values))
        self.assertEqual([1,3,5,7,9],odds)
        
        evens = list(filter(lambda x : x%2==0, values))
        self.assertEqual([0,2,4,6,8],evens)
        
    def testMapSingleVariate(self):
        it = map(lambda x: x*x,(6,7,8))
        self.assertEqual(36,next(it))
        
        remainings = list(it)
        self.assertEqual([49,64],remainings)
        
    def testMapMultiVariate(self):
        # only requirement for argument is it must be iterable, no need to keep same type
        # here () is necessary to encapsulate (x+y,x-y), otherwise lambda will only recognize "x+y"
        results = list(map(lambda x,y: (x+y,x-y),(1,2,3),[4,5,9]))
        self.assertEqual([(5,-3),(7,-3),(12,-6)],results)
        
    def testReduce_NoInit(self):
        self.assertEqual(10,functools.reduce(lambda x,y: x+y,range(5)))
        
    def testReduce_WithInit(self):
        # if initial value is missing, first action happens on arguments[0] and arguments[1]
        self.assertEqual("helloworld ",functools.reduce(lambda x,y: x+y+" ",("hello","world")))
        
        # if initial value is given, first action happens on that initial value and arguments[0]
        self.assertEqual("cheka said: hello world ",functools.reduce(lambda x,y: x+y+" ",("hello","world"),"cheka said: "))
        
    def testLazyEvaluatedGenerator(self):
        sideeffect = [0]
        def fool(x):
            sideeffect[0] += 1
            return x * x
        
        a = [1,2,3]
        gen = map(fool,a)# just create a view, not evaluated yet
        
        self.assertEqual(0,sideeffect[0])
        
        list1 = list(gen)
        self.assertEqual([1,4,9],list1)
        self.assertEqual(3,sideeffect[0])
        
        # it is a generator, which can be executed only once
        list2 = list(gen)
        self.assertEqual(0,len(list2))
        self.assertEqual(3,sideeffect[0])
        
        
        
if __name__ == "__main__":
    unittest.main()