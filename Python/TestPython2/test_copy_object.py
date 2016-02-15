
import unittest
import copy

class ObjectCopyTest(unittest.TestCase):
    # ============================== helper
    class _Manager(object):
        def __init__(self,name):
            self._name = name
            self._workers = []
            
        def addWorker(self,worker):
            self._workers.append(worker)
            
        def getWorker(self,index):
            return self._workers[index]
            
    class _Worker(object):
        def __init__(self,name):
            self._name = name
            
        def __eq__(self,other):
            if (not isinstance(other,ObjectCopyTest._Worker)):
                return False
            elif self is other:
                return True
            else :
                return self._name == other._name

    def makeManager(num_workers):
        mgr = ObjectCopyTest._Manager("cheka")
        for index in range(num_workers):
            mgr.addWorker(ObjectCopyTest._Worker("worker-%d"%(index + 1)))
        return mgr
    makeManager = staticmethod(makeManager)
            
    # ============================== test
    def testDeepCopy(self):
        num_workers = 6
        srcmgr = ObjectCopyTest.makeManager(num_workers)
        cpymgr = copy.deepcopy(srcmgr)
        
        # -------------------- immutable member field not copied
        self.assertTrue(srcmgr._name,cpymgr._name)
            
        # -------------------- mutable member field are copied
        self.assertTrue(srcmgr._workers is not cpymgr._workers)
        
        # !!!!!!!!!!!!!!!!!!!! deep copy is recursive, not only mutable member field is copied
        # !!!!!!!!!!!!!!!!!!!! but also "mutable member of that mutable member field" is also copied
        for index in range(num_workers):
            srcworker = srcmgr.getWorker(index)
            cpyworker = cpymgr.getWorker(index)
            
            self.assertEqual(srcworker,cpyworker)
            self.assertTrue(srcworker is not cpyworker)

if __name__ == "__main__":
    unittest.main()