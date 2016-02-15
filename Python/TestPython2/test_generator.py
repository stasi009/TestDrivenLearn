
import unittest

class GeneratorTest(unittest.TestCase):
    """
    wrap the "list comprehension" with [], then it is "list comprehension"
    wrap the "list comprehension" with (), then it is "generator"
    one big difference between generator and list comprehension is that generator is lazy
    """

    def test_xrange(self):
        self.assertEqual([1,2,3],list(xrange(1,4)))# exclude the end
        self.assertEqual([0,1,2],list(xrange(3)))# defaults start from 0
        self.assertEqual([4,3,2,1],list( xrange(4,0,-1) ))# negative stride to loop backwards
    
    def testListActive(self):
        # python2 doesn't have nonlocal keyword
        # so we have to use a list type instead
        sideeffect = [0]
        
        def mapper(n):
            # must use list, otherwise, it will create a new variable with same name
            # and will throw "UnboundLocalError" because reference before assignment 
            sideeffect[0] += 1
            return n*n
        
        alist = [mapper(x) for x in range(1,5)]
        self.assertEqual(4,sideeffect[0])
        self.assertEqual([1,4,9,16],alist)
        
    def testGeneratorLazy(self):
        sideeffect = [0]
        
        def mapper(n):
            sideeffect[0] += 1
            return n*n
        
        agenerator = (mapper(x) for x in range(1,5))
        self.assertEqual(0,sideeffect[0]) # generator not invoked yet, so still no side effect
        
        self.assertEqual([1,4,9,16],list(agenerator))    
        self.assertEqual(4,sideeffect[0]) # generator invoked, make effects
        
        # already iterated once, become empty
        self.assertEqual([],list(agenerator))    
    
    def testAny(self):
        """
        generator expression is much like list comprehension
        but return an iterator, other than a concrete list which may occupies a lot of memory
        generator expression has much advantages in memory usage
        """
        orilist = [1,3,5,7,8]
        is_even_generator = ((x%2==0) for x in orilist)
        self.assertTrue(any(is_even_generator))
        
    def testAll(self):
        """
        !!!!!!! pay much attention that even this testcase has only one line different from above one
        !!!!!!! but it cannot combine with the previous testcase and become a single testcase
        !!!!!!! the reason is that "Generator" cannot be re-used, which is much different from LINQ
        !!!!!!! Generator is a state-machine, after reaching its end, no more element can be generated out
        """
        orilist = [1,3,5,7,8]
        is_even_generator = ((x%2==0) for x in orilist)
        self.assertFalse(all(is_even_generator))
        
    def testUnableReuse(self):
        generator = (x for x in xrange(3))
        
        first_list = []
        for x in generator:
            first_list.append(x)
        self.assertEqual(range(3),first_list)
        
        # same generator cannot be used again
        second_list = list(generator)
        self.assertEqual(0,len(second_list))
        
    def testUnableReuse2(self):
        def simple_generator():
            yield 1
            yield "cheka"
        generator = simple_generator()
        
        # ------------- first time use that generator
        list1 = list(generator)
        self.assertEqual([1,"cheka"],list1)
        
        # ------------- first time use that generator
        list2 = []
        for x in generator:
            list2.append(x)
        self.assertEqual(0,len(list2))
        
    def testBuiltinNext(self):
        """
        generator.next() is removed from Python 3.3, so the only way left is to use 'next(generator)' builtin function
        """
        def sample_generator():
            yield "cheka"
            yield 1
        gen = sample_generator()
        
        self.assertEqual("cheka",next(gen))
        self.assertEqual(1,next(gen))
        self.assertRaises(StopIteration,lambda : next(gen) )
        
    def testLazyFeature(self) :
        class FoolGenerator(object):
            def __init__(self,total):
                self.counter = 0
                self._total = total
                
            def generate(self):
                for index in range(1,self._total+1):
                    self.counter += 1
                    yield index*index
                    
                    
        fool = FoolGenerator(10)
        self.assertEqual(0,fool.counter)
        
        generator = fool.generate()
        self.assertEqual(1,next(generator))
        self.assertEqual(4,next(generator))
        self.assertEqual(2,fool.counter)
        
        self.assertTrue(16 in generator)
        self.assertEqual(4,fool.counter)
        
        # since 4 has already been popped out from the generator
        # so there is no "4" any more
        # and we have to iterate all to get that negative answer
        # so "side-effect" occur 10 times
        self.assertFalse(4 in generator)
        self.assertEqual(10,fool.counter)
    

    def testSend(self):
        """
        not only can retrieve data from generator, but can inject data into generator
        """
        # ----------------- make duplex generator
        def _simple_duplex_generator(start = 0):
            count = start
            while True:
                injected = (yield count)
                if injected is not None:
                    count = injected
                else:
                    count += 1
                    
        # ----------------- make duplex generator
        generator = _simple_duplex_generator(4)
        self.assertEqual(4,next(generator))
        self.assertEqual(5,next(generator))
        
        self.assertEqual(10, generator.send(10) )
        self.assertEqual(11, next(generator))
        self.assertEqual(12, next(generator))
        
        generator.close()
        self.assertRaises(StopIteration,lambda:  next(generator))
            
        
if __name__ == "__main__":
    unittest.main()
        
    
    