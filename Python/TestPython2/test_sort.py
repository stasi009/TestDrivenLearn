
import unittest
import packages1.person as person

class SortTest(unittest.TestCase):

    class Student(object):
        def __init__(self,number,name):
            self._no = number
            self._name = name
            
        def getNumber(self): return self._no
        def getName(self): return self._name
        
    def setUp(self):
        self._stud1 = SortTest.Student(1,"cheka")
        self._stud2 = SortTest.Student(2,"stasi")
        self._stud3 = SortTest.Student(3,"mss")
        
        self._unsorted = [self._stud3,self._stud2,self._stud1] 
        self._number_sorted = [self._stud1,self._stud2,self._stud3]
        self._name_sorted = [self._stud1,self._stud3,self._stud2]
    
    def testSorted_ByKey(self):
        sorted_by_no = sorted(self._unsorted,key=SortTest.Student.getNumber)
        self.assertEqual(self._number_sorted,sorted_by_no)
        
        sorted_by_name = sorted(self._unsorted,key=SortTest.Student.getName)
        self.assertEqual(self._name_sorted,sorted_by_name)
        
    def testSorted_ByKey2(self):
        sorted_by_no = sorted(self._unsorted,key=lambda x: x.getNumber())
        self.assertEqual(self._number_sorted,sorted_by_no)
        
        sorted_by_name = sorted(self._unsorted,key=lambda x: x.getName(),reverse=True)
        self._name_sorted.reverse() # reverse in place
        self.assertEqual(self._name_sorted,sorted_by_name)    
        
    def testSortGeneralIterable(self):
        """
        just to show that, sorted can be used not only on list
        but can work on any iterables
        """
        adict = dict(map(lambda s: (s.getNumber(),s.getName()),self._unsorted))
        sortlist = sorted(adict.items(),key=lambda t:t[0],reverse=True)
        self.assertEqual([(3,"mss"),(2,"stasi"),(1,"cheka")],sortlist)
        
    def testInplaceSort(self):
        alist = [person.Person(x,"p%d"%x) for x in xrange(4,0,-1)]
        ascending_list = [person.Person(x,"p%d"%x) for x in xrange(1,5)] 
        self.assertNotEqual(ascending_list,alist)
        
        alist.sort(key = lambda x : x._ssn)
        self.assertEqual(ascending_list,alist)
        
    def testInplaceSort2(self):
        alist = [1,4,3,2]
        alist.sort()
        self.assertEqual([1,2,3,4],alist)
        
        alist.sort(key=lambda x: -x)
        self.assertEqual([4,3,2,1],alist)
        
if __name__ == "__main__":
    unittest.main()
