
import unittest

class DictTest(unittest.TestCase):
    
    def testConstruct(self):
        expected_dict = {1:"cheka",2:"stasi"}
        
        # ---------- construct from list comprehension
    	original_dict = {"cheka":1,"stasi":2}
        dict_from_lstcomph = { v:k for k,v in original_dict} 
        self.assertEqual(expected_dict,dict_from_lstcomph)

        # ---------- construct from sequence
        dict_from_tuple = dict(  ([1,"cheka"],[2,"stasi"])  ) 
        self.assertEqual(expected_dict,dict_from_tuple)
        
        dict_from_list = dict(  ( (1,"cheka"),[2,"stasi"] )  ) 
        self.assertEqual(expected_dict,dict_from_list)
        
        # ---------- construct from default values 
        dict_from_defvalues = {}.fromkeys((1,2),"")
        self.assertEqual({1:"",2:""},dict_from_defvalues)
        
        dict_fromkeys = dict.fromkeys((1,2))
        self.assertEqual({1:None,2:None},dict_fromkeys)
        
        # ---------- construct from zip
        self.assertEqual( expected_dict,dict(zip((1,2),("cheka","stasi"))) )
        
    def testNokeyException(self):
        self.assertRaises(KeyError,lambda : {}["nonexisted"])
        
    def testEqual(self):
        dict1 = {1:"cheka","stasi":2}
        dict2 = {1:"cheka","stasi":2} # mutable type always create a totally new instance
        
        self.assertTrue(dict1 == dict2)
        self.assertTrue(dict1 is not dict2)
        
        # ---- order when being initialized will not effect equality
        # ---- because dictionary will not keep order
        dict_diff_order = {"stasi":2,1:"cheka"}
        self.assertEqual(dict1,dict_diff_order)
        
    def testIn(self):
        """
        has_key is obsolete, so it is recommended to check whether a key exists
        in the dictionary by using "in" or "not in"
        """
        adict = {1:"cheka","stasi":100.0}
        self.assertTrue(1 in adict)
        self.assertTrue("stasi" in adict)
        self.assertTrue("nokey" not in adict)
        
    def testCopy(self):
        oridict = {1:[]}
        cpydict = oridict.copy()
        
        self.assertEqual(oridict,cpydict)
        self.assertTrue(oridict is not cpydict)
        
        self.assertTrue(oridict[1] is cpydict[1]) # just a shallow copy
        oridict[1].append("stasi")
        self.assertEqual(["stasi"],cpydict[1])
        
    def testRemove(self):
        # ---------------------- remove by "del"
        adict = {1:"cheka","stasi":2,"mss":100.0}
        del adict["mss"]
        self.assertEqual({1:"cheka","stasi":2},adict)
        
        # ---------------------- remove by "pop"
        self.assertEqual("cheka",adict.pop(1))
        self.assertEqual({"stasi":2},adict)
        
        # raise exception when key not existed
        self.assertRaises(KeyError,lambda : adict.pop("nonexisted"))
        
        # return default value when key not existed and default value is set
        self.assertEqual("default",adict.pop("nonexisted","default"))
        
        # ---------------------- remove by "clear"
        adict.clear()
        self.assertEqual(0,len(adict))
    
    def testGet(self):
        adict = {1:"cheka"}
        self.assertEqual("cheka",adict[1])
        
        self.assertRaises(KeyError,lambda : adict[0])
        
        self.assertEqual("default",adict.get(2,"default"))
        self.assertEqual("cheka",adict.get(1,"default"))
        
        # ----------- if default value is not given, just return None, other than throw exception
        self.assertTrue(adict.get("nokey") is None)
        self.assertIsNone(adict.get("nokey"))

    def test_setdefault(self):
        """
        similar to get, both method return defaultvalue if key is not found
        but setdefault is different from get, that, it will insert that <NotFoundKey,DefaultValue> into the dictionary
        """
        adict = {1:"cheka"}
        self.assertEqual("x",adict.get(2,"x"))
        # default value isn't inserted into dict, so the next time, it still cannot be found and return the default value
        self.assertEqual([1],adict.keys())

        # setdefault will insert that <key,default> pair
        # so the key will be found next time
        self.assertEqual("cheka",adict.setdefault(1,"y"))
        self.assertEqual("y",adict.setdefault(2,"y"))
        self.assertEqual([1,2],adict.keys())

    def testUpdate(self):
        """
        unlike in C#, where add with existed key will throw exception
        but in Python, no builtin way to notify you the key has existed
        if you provide the same key, you are upating old values with new values
        you have to use "in" to check whehter the key has existed or not yourself
        """
        adict = {}
        
        key = 1
        ori_value = "cheka"
        
        # -------------------- first write by key is adding
        adict[key] = ori_value
        self.assertEqual(1,len(adict))
        self.assertEqual(ori_value,adict[key])
        
        # -------------------- following write by key is updating
        new_value = "stasi"
        adict[key] = new_value
        self.assertEqual(1,len(adict))
        self.assertEqual(new_value,adict[key])
        
    def testUpdate2(self):
        adict = {1:"cheka"}
        adict.update({"stasi":2})
        self.assertEqual({1:"cheka","stasi":2},adict)
        
        # --------------- old values will be overriden by new values with duplicated key
        adict.update({"stasi":"newvalue"})
        self.assertEqual({1:"cheka","stasi":"newvalue"},adict)
        
    def testHybridTypes(self):
        """neither the key or the value must be of the same type"""
        hybrid_dict = {1:"cheka","stasi":100.0,(99,"stasi"):9999}
        self.assertEqual("cheka",hybrid_dict[1])
        self.assertAlmostEqual(100.0,hybrid_dict["stasi"])
        self.assertTrue((99,"stasi") in hybrid_dict) # composite key

    def testKeys(self):
        adict = {1:"stasi",9:"cheka"}

        listkeys = adict.keys()
        self.assertIsInstance(listkeys,list)
        self.assertEqual([1,9],listkeys)

        viewkeys = adict.viewkeys()
        # viewkeys is not list, not is iterable, so can be converted into list
        self.assertEqual([1,9],list(viewkeys))

        # the difference between keys and viewkeys is that
        # view is dynamic, will reflect the changes on the original dictionary
        del adict[1]
        self.assertEqual([1,9],listkeys)
        self.assertEqual([9],list(viewkeys))

    def test_list_comprehension(self):
        d = {1:"a",2:"b"}
        reverse_dict = {v:k for k,v in d.viewitems() }
        self.assertEqual({"a":1,"b":2},reverse_dict)
        
    def testViewObjects(self):
        """
        viewkeys(), viewvalues(), viewitems() return dynamic view
        when changes are made on the dictionary, the view will reflect those changes
        """        
        adict = {1:"stasi",9:"cheka"}
        keyview = adict.viewkeys()
        valueview = adict.viewvalues()
        
        self.assertNotIsInstance(keyview,list)
        self.assertFalse(isinstance(valueview,list))
        
        # view can be iterated
        self.assertItemsEqual([1,9],[k for k in keyview])
        self.assertItemsEqual(("stasi","cheka"),valueview)
        
        # modification on the dictionary be reflected on the view
        del adict[9]
        self.assertEqual([1],list(keyview)) # not necessary to re-construct the key view
        self.assertEqual(("stasi",),tuple(valueview))
        
    def testIterate(self):
        """
        iterate a map, it only iterate its keys
        if you want to iterate key-value-pair, you should use dict.items()
        """
        d = {6:"stasi",9:"cheka"}
        keys = sorted([i for i in d])
        self.assertEqual([6,9],keys)
        self.assertEqual( ["cheka","stasi"],sorted(v for (k,v) in d.items()))     

    def testSimulateSwtichCase(self):
        choice = {
            "choice1":"action1",
            "choice2":"action2"
        }
        default_action = "default"
        self.assertEqual("action1",choice.get("choice1",default_action))
        self.assertEqual("default",choice.get("unrecognized",default_action))

if __name__ == "__main__":
    unittest.main()
