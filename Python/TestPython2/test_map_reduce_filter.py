
import unittest
# in python3, 'reduce' has been moved into functools module
# but in python2, 'reduce' is still built-in function
# import functools
class MapReduceFilterTest(unittest.TestCase):
    
    def testFilter(self):
        values = range(10)
        
        odds = filter(lambda x : x % 2 == 1,values)
        self.assertEqual([1,3,5,7,9],odds)
        
        evens = filter(lambda x : x % 2 == 0, values)
        self.assertEqual([0,2,4,6,8],evens)
        
    def testMapMultiVariate(self):
        # only requirement for argument is it must be iterable, no need to keep same type
        # here () is necessary to encapsulate (x+y,x-y), otherwise lambda will
        # only recognize "x+y"
        results = map(lambda x,y: (x + y,x - y),(1,2,3),[4,5,9])
        self.assertEqual([(5,-3),(7,-3),(12,-6)],results)

    def testUseReduceCountLen(self):
        # python doesn't have Seq.length function
        # we can use reduce to count the length of an iterator in one line
        it = iter("stasi")
        length = reduce(lambda a,e: a+1,it,0)
        self.assertEqual(5,length)
        
    def testReduce_NoInit(self):
        self.assertEqual(10,reduce(lambda a,e: a + e,xrange(5)))
        
    def testReduce_WithInit(self):
        # if initial value is missing, first action happens on arguments[0] and arguments[1]
        self.assertEqual("helloworld ",reduce(lambda a,e: a + e + " ",("hello","world")))
        
        # if initial value is given, first action happens on that initial value and arguments[0]
        self.assertEqual("cheka said: hello world ",reduce(lambda a,e: a + e + " ",("hello","world"),"cheka said: "))
        
    def testReturnCollectionNotGenerator(self):
        sideeffect = [0]
        def fool(x):
            sideeffect[0] += 1
            return x * x
        
        # even the passed in argument is a generator, "map" also return a concrete list
        a = xrange(1,5)
        list1 = map(fool,a)
        self.assertEqual(4,sideeffect[0])# has already been evaluated
        self.assertEqual([1,4,9,16],list1)
        
if __name__ == "__main__":
    unittest.main()