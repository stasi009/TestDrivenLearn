
import unittest

class ListTest(unittest.TestCase):
    
    def testCreateFromIterable(self):
        """
        this test shows that when list() is given a iterable object
        list will create a list and put each item in that iterable object into that created list
        """
        self.assertEqual(["c","h","e","k","a"],list("cheka"))
    
    def testMultipleTypes(self):
        info = [1,"stasi",99.9]
        self.assertEqual(info[0],1,"first element is an integer")
        self.assertEqual(info[1],"stasi","second element is a string")
        self.assertAlmostEqual(info[2],99.9,"third element is a float")
        
    def testAccessByIndex(self):
        info = [2,"cheka",88.8]
        
        self.assertEqual(2,info[0])
        self.assertEqual(2,info[-3])
        self.assertAlmostEqual(88.8,info[-1])
        
        # index out of range
        with self.assertRaises(IndexError):
            info[77]
            
        with self.assertRaises(IndexError):
            info[-4]
        
    def testIndex(self):
        alist = [1,"cheka",1,"cheka",1,3,1]
        self.assertEqual(0,alist.index(1))
        # the second parameter specifies the position where search starts
        self.assertEqual(2,alist.index(1,1)) # always find the first position "k" where alist[k]==wanted,where start<=k<stop
        
        self.assertEqual(1,alist.index("cheka"),msg="find from position 0 to len(alist)")
        self.assertEqual(3,alist.index("cheka",2),msg="find from position 2 to len(alist)")
        
        self.assertRaises( ValueError, lambda : alist.index("NonExisted") )    
        
    def testEquality(self):
        """
        this testcase proves that for list object
        '==' checks equality based on content checking, rather that reference checking
        """
        list1 = [1,8,9]
        list2 = [1,8,9]
        
        # not the same object
        self.assertIsNot(list1,list2)
        self.assertNotEqual(id(list1),id(list2))
        self.assertFalse(list1 is list2)
        
        # but content is equal
        self.assertEqual(list1,list2)
        self.assertTrue(list1 == list2)
        
        ### 
        list1 = []
        list2 = []
        self.assertFalse(list1 is list2)    
        self.assertEqual(list1,list2)
        
    def testIn(self):
        list1 = [1,"stasi"]
        self.assertTrue(1 in list1)
        self.assertTrue("stasi" in list1)
        self.assertFalse(999 in list1)
        
    def testLength(self):
        self.assertEqual(3,len([1,"stasi",99.9]))
        
    def testSort(self):
        """
        difference between "list.sort" and built-in "sorted" is that
        "list.sort" sort the list in place
        while built-in "sorted" return a new list sorted by specific order
        """
        numlist = [6,4.78,1.2,5]
        numlist.sort()
        self.assertEqual([1.2,4.78,5,6],numlist)
        
        strlist = ["kgb","mss","cheka"]
        strlist.sort()
        self.assertEqual(["cheka","kgb","mss"],strlist)           
        
        # ------------ reverse sort
        numlist.sort(reverse = True)
        self.assertEqual([6,5,4.78,1.2],numlist)
        
    def testSortBykey(self):
        names = ["cheka","STASI","mss"]
        names.sort()
        self.assertEqual(["STASI","cheka","mss"],names)
        
        names.sort(key = str.lower)
        self.assertEqual(["cheka","mss","STASI"],names)   
        
    def testReplace(self):
        """
        when replacing, the left handside is a single [index] or [startindex:endindex] is totally different
        alist[idnex] = [x,y] ===> alist[... [x,y], ...]
        alist[startindex:endindex] = [x,y] ===> alist[... x,y, ...]
        """
        alist = [1,"cheka",100.0,"stasi",99]
        
        # when subsitute a single item with empty list or empty tuple
        # it just replace that single element
        alist[1] = [] 
        self.assertEqual([1,[],100.0,"stasi",99],alist)
        
        # when subsitute a range with empty list or empty tuple
        # it equivalent to delete
        alist[1:2] = ()
        self.assertEqual([1,100.0,"stasi",99],alist)
        
        alist[1:3] = []
        self.assertEqual([1,99],alist)
        
        # when replace a range, the right hand-side can be any iterable
        alist[0:1] = (5,"new",2)
        self.assertEqual([5,"new",2,99],alist)
        
        alist[1:3] = "python"
        self.assertEqual([5,"p","y","t","h","o","n",99],alist)
        
        alist[1] = "cheka"
        self.assertEqual([5,"cheka","y","t","h","o","n",99],alist)  
        
    def testReplace2(self):
        alist = [1,2,3,4]
        alist[1:-1] = ["stasi"]
        self.assertEqual([1,"stasi",4],alist)
        
        alist = [1,2,3,4]
        reference = alist
        alist[1:-1] = "stasi"
        self.assertEqual([1,"s","t","a","s","i",4],alist)  
        
        # replace all
        alist[:] = "ok"
        self.assertEqual(["o","k"],alist)

        # all the changes happen in place, so no new copy will be allocated
        self.assertIs(alist,reference)
        self.assertEqual(["o","k"],reference)
        
    def testCopy(self):
        srclist = [1,"stasi",[3,4]]
        cpylist = srclist[:]
        
        self.assertEqual(srclist,cpylist)
        self.assertIsNot(srclist,cpylist)
        
        # still a shallow copy
        cpylist[0] = 999
        self.assertEqual(1,srclist[0]) # not changed
        
        cpylist[2].append(5)
        self.assertEqual([3,4,5],srclist[2])
        
    def testSlice(self):
        alist = [0,1,2,3,4,5]
        ### brackets are inclusive on the left 
        ### but exclusive on the right
        self.assertEqual([0,1],alist[0:2],"list[a,b] is actually list[a,b)")
        self.assertEqual([0,1,2],alist[:3])
        
        acopy = alist[:]
        self.assertEqual(acopy,alist)
        self.assertIsNot(acopy,alist)
        
        ### use reverse negative index
        self.assertEqual([4],alist[-2:-1])
        self.assertEqual([0,1,2],alist[:-3])
        self.assertEqual([3,4,5],alist[-3:])
        
        ### use stride
        alist = range(10)
        self.assertEqual([0, 2, 4, 6, 8],alist[::2])
        self.assertEqual([1, 3, 5],alist[1:7:2])
        
    def testSlice2(self):
        alist = [100,"cheka",66]
        self.assertEqual(66,alist[-1])
        self.assertEqual(["cheka"],alist[1:-1])
        self.assertEqual([100,"cheka"],alist[:-1])
        
        # -------------- check that slice returns a totally new instance
        # -------------- not just a reference to the original list
        part_list = alist[:-1]
        part_list[0] = "new element"
        # since slice returns new, isolated instance, so change on the slice will not affect original list
        self.assertEqual(["new element","cheka"],part_list)
        self.assertEqual([100,"cheka",66],alist)    
        
    def testSliceInverse(self):
        a = [1,2,3,4,5]
        self.assertEqual([3,4],a[-3:-1])
        self.assertEqual([],a[-1:-3])
        self.assertEqual([5,4],a[-1:-3:-1])
        
    def testMutable(self):
        """element in the lsit can be changed"""
        alist = [1,2,3]
        aref = alist
        alist[1] = "stasi"
        self.assertEqual([1,"stasi",3],aref)
        
    def testPop(self):
        # when no argument is given, default argument for "pop" is -1, standing for the last item
        alist = [1,2,"cheka",3,"stasi"]
        self.assertEqual("stasi",alist.pop())
        self.assertEqual([1,2,"cheka",3],alist)
        
        self.assertEqual(2,alist.pop(1))
        self.assertEqual([1,"cheka",3],alist)    
        
    def testReferenceFeature(self):
        """list is mutable type, has the 'reference' nature"""
        alist = [1]
        aref = alist 
        alist.append("stasi")
        self.assertEqual([1,"stasi"],aref)
        self.assertIs(aref,alist)
        
    def testAssign(self):
        alist = [1,2,3]
        
        # if assign a single position, then the value can be a single value
        alist[1] = 88
        self.assertEqual([1,88,3],alist)
        
        # if assign a range, then the value must be iterable
        with self.assertRaises(TypeError): alist[1:2] = 99
        
    def testAdd(self):
        record1 = [1,"cheka",100.0]
        record2 = [2,"stasi",99.9]
        recordsum = record1 + record2
        
        self.assertEqual([1,"cheka",100.0,2,"stasi",99.9],recordsum)
        self.assertIsNot(recordsum,record1)
        self.assertIsNot(recordsum,record2)
        
    def testPrepend(self):
        alist = [1,2,3]
        alist[:0] = [-1,0]
        self.assertEqual([-1,0,1,2,3],alist)
    
    def testAppend(self):
        alist = [1]
        alist.append("cheka") # iterable element is appended as a whole object
        alist.append([2,100.0])
        self.assertEqual([1,"cheka",[2,100.0]],alist)
        
        with self.assertRaises(TypeError,msg="if a range inside [], then the value must also be iterable, not a single value"):
            alist[len(alist):] = 1
        
        # ---------- "alist.append(x)" is equivalent to "alist[len(alist):]=[x](or (x))"
        # ---------- we must put x inside [], then it will just add element 'x', rather than [x]
        alist[len(alist):] = ["new"]
        self.assertEqual([1,"cheka",[2,100.0],"new"],alist)   
        
        alist[len(alist):] = "new"
        self.assertEqual([1,"cheka",[2,100.0],"new","n","e","w"],alist)     
        
    def testClear(self):
        def workingClear(ilist): del ilist[:]
        def brokenClear(ilist): ilist = [] # Lets ilist point to a new list, losing the reference to the argument list
        
        list1=[1, 2]; 
        workingClear(list1); 
        self.assertEqual(0,len(list1))
        
        list1=[1, 2]; 
        brokenClear(list1); 
        self.assertEqual([1,2],list1)
        
    def testCount(self):
        alist = [1,"cheka",1,"cheka",1,3,1]
        self.assertEqual(4,alist.count(1))
        self.assertEqual(2,alist.count("cheka"))
        self.assertEqual(1,alist.count(3))
        self.assertEqual(0,alist.count("NonExisted"))     
        
    def testDel(self):
        alist = [1,"cheka",100.0,"stasi",99]
        
        # --------- delete single
        del alist[1]
        self.assertEqual([1,100.0,"stasi",99],alist)
        
        # --------- delete a range
        del alist[1:3]
        self.assertEqual([1,99],alist)
        
        # --------- delete equivalent to replace with []
        alist[0:1] = []
        self.assertEqual([99],alist) 
        
        # --------- Clear AKA empty AKA erase
        alist = [1,"cheka",100.0,"stasi",99]
        self.assertEqual(5,len(alist))
        
        del alist[:]
        self.assertEqual(0,len(alist))
        
    def testDel2(self):
        alist = list(range(4)) # now range() is a generator, becomes immutable, so we have to transform it into list first
        aref = alist
        original_id = id(alist)
        
        del alist[:2]
        self.assertEqual([2,3],aref) # modify directly in place, not on a copy
        
        del alist[:] # clear all
        self.assertEqual(0,len(aref))
        
        current_id = id(alist) 
        self.assertEqual(original_id,current_id)    
        
    def testRemove(self):
        alist = ["cheka","stasi","cheka"]
        alist.remove("cheka") # only remove the first matched item
        self.assertEqual(["stasi","cheka"],alist) 
        
    def testMultiply(self):
        # =============== simple test
        self.assertEqual([1,1,1],[1]*3)
        self.assertEqual(["stasi","stasi"],["stasi"]*2)
        
        # =============== prove multiply is just shallow copy
        class SimpleObject(object):
            def __init__(self):
                self.id = 0
                
        # =============== test
        objlist = [SimpleObject()] * 2
        
        # just shallow copy, not deep copy
        self.assertTrue(objlist[0] is objlist[1])   
        
    def testMultiplyAndReference(self):
        ####################### reshared reference
        reshared = [[0]*2]*2
        self.assertEqual([[0,0],[0,0]],reshared)
        
        reshared[0][0] = 1
        self.assertEqual([[1,0],[1,0]],reshared)
        
        ####################### isolated copies
        copies = [[0]*2 for x in range(2)]
        self.assertEqual([[0,0],[0,0]],copies)
        
        copies[0][0] = 1
        self.assertEqual([[1,0],[0,0]],copies)
        
    def testInsert(self):
        alist = [1,2,3]
        alist.insert(1,"cheka")
        self.assertEqual([1,"cheka",2,3],alist)
        
    def testExtend(self):
        alist = [1,"cheka"]
        alist.extend("ok")
        self.assertEqual([1,"cheka","o","k"],alist)
        
        # ---------- "extend" is equivalent to "alist[len(alist):] = x",where x is iterable
        alist[len(alist):] = [45,"new"]
        self.assertEqual([1,"cheka","o","k",45,"new"],alist)
        
    def testDiffExtendAndPlus(self):
        # ---------- "+" can also extend, but will create a new object to hold the concatenate result
        alist = [1]
        oriid = id(alist)
        
        # throw exception due to argument is not iterable
        self.assertRaises( TypeError, lambda : alist.extend(2))
        
        alist.extend([2])
        newid = id(alist)
        self.assertEqual(oriid,newid)
        
        alist = alist + [3]
        self.assertEqual([1,2,3],alist)
        newid = id(alist)
        # using "extend" will just change the same list, different from "+", which will create a new instance
        # and copy data from both parameter into that new instance, so "extend" is more efficient
        self.assertNotEqual(oriid,newid)
        
        # ---------- and "extend" can take a argument which is not the same type, as long as that object is iterable
        # ---------- but "+" cannot
        alist.extend("ok")
        self.assertEqual([1,2,3,"o","k"],alist)
        
        self.assertRaises(TypeError,lambda: alist + "ok") # only same type can "+"
    
        
if __name__ == "__main__":
    unittest.main()