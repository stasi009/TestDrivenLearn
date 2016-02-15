
import unittest
import copy

class ContainerCopyTest(unittest.TestCase):
    # ============================================================ #
    # shallow copy
    # ============================================================ #
    def _checkShallowCopy(self,copy_func):
        """
        shallow copy is just create a totally new instance, but its member are just referencing the same member as the original instance
        (that is to say, two different container, but items hold are the same,
        then for immutable item, modify immutable item through copy container, will let that immutable item point to another new instance,
        but for mutable item, modify mutable item through one name, will affect the other)
        
        there are three ways to do shallow copy:
        1. fully slice: cpylist = srclist[:]
        2. using constructor: cpylist = list(srclist) 
        3. using copy.copy
        """
        # --------------------------------- make and copy original
        srclist = [1,"cheka",[2,"stasi"]]
        cpylist = copy_func(srclist)
        
        # --------------------------------- check equal
        self.assertEqual(srclist,cpylist)
        
        # --------------------------------- check container itself totally new
        self.assertTrue(srclist is not cpylist)
    
        # --------------------------------- check items in the container referencing the same object
        size = len(srclist)
        for index in range(size):
            self.assertTrue(srclist[index] is cpylist[index])
            
        # --------------------------------- modify immutable item, resulting in pointing to new object
        cpylist[1] *= 2
        self.assertEqual([1,"chekacheka",[2,"stasi"]],cpylist) # immutable element in the source list not changed
        self.assertEqual([1,"cheka",[2,"stasi"]],srclist) # immutable element in the source list not changed
        
        # --------------------------------- modify mutable item, affect original ones
        cpylist[2][0] *= 100
        self.assertEqual([200,"stasi"],srclist[2])
        
        
    def testShallowcopyBySlice(self):
        self._checkShallowCopy(lambda x: x[:])
        
    def testShallowcopyByConstructor(self):
        self._checkShallowCopy(lambda x: list(x))
        
    def testShallowcopyByCopy(self):
        self._checkShallowCopy(lambda x: copy.copy(x))
    
    # ============================================================ #
    # deep copy
    # ============================================================ #
    def testDeepCopy(self):
        """
        when a container is deep-copied, not only the container itself are copied
        but the mutable items in the container is also copied, but the immutable items are not copied nor necessary
        """
        srclist = [1,"cheka",[2,"stasi"]]
        cpylist = copy.deepcopy(srclist)
        
        # --------------------------------- test equal
        self.assertEqual(srclist,cpylist)
        
        # --------------------------------- test not same
        self.assertTrue(srclist is not cpylist)
        
        # --------------------------------- test items same or not
        self.assertTrue(srclist[0] is cpylist[0])
        self.assertTrue(srclist[1] is cpylist[1])
        
        self.assertTrue(srclist[2] is not cpylist[2])
        cpylist[2][0] *= 100
        self.assertEqual([200,"stasi"],cpylist[2])     
        self.assertEqual([2,"stasi"],srclist[2]) # not changed 
           
    
if __name__ == "__main__":
    unittest.main()