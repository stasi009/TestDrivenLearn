
import unittest

class GroupedArgsTest(unittest.TestCase):
    # ---------------------- for test list arguments
    def _sum(self,*args):
        size = len(args)
        
        sum = args[0]
        for index in range(1,size):
            sum += args[index]
        return sum
    
    def testTupleParameter(self):
        self.assertEqual(6,self._sum(1,2,3))
        self.assertEqual("chekakgbstasi",self._sum("cheka","kgb","stasi"))
        
    # ---------------------- for test dictionary arguments
    def _splitKeysValues(self,keys,values,**kwargs):
        del keys[:]
        del values[:]
        
        for kv in kwargs.items():
            keys.append(kv[0])
            values.append(kv[1])
            
        keys.sort()
        values.sort()
    
    def testDictParameters(self):
        # set some initial values, to test the effect of clearing inside the method
        keys = list(range(3))
        values = list(range(5))
        
        # chekanote: to pass as "keyword argument", passing as "key=value" format
        # and in the method body, all key-word arguments will be grouped into a dictionary
        # where the key is like "key"
        self._splitKeysValues(keys,values,stasi = 100,cheka=10,kgb=2,cia=3)
        
        self.assertEqual(["cheka","cia","kgb","stasi"],keys)
        self.assertEqual([2,3,10,100],values)
        
    def testSplitArguments(self):
        """
        when used outside method to invoke function with grouped parameter
        "*" or "**" works as spliter, split a collection into individual segments and pass into that method with grouped parameter
        """
        # ----------- split list 
        # ----------- without, (1,2,3) is passed as a single element, then inside method, args is a collection with only one argument
        self.assertEqual((1,2,3),self._sum((1,2,3)))
        self.assertEqual(18,self._sum(*(5,6,7)))
        
        # ----------- split dictionary
        keys = [1]
        values = [None]
        
        # chekanote: below codes throw TypeError exception, due to "keyword can only be string" 
        # self._split_keys_values(keys,values,**{2:"stasi",1:"cheka"})
        self._splitKeysValues(keys,values,**{"stasi":2,"cheka":1})
        self.assertEqual([1,2],values)
        self.assertEqual(["cheka","stasi"],keys)


    def testDefaultExtendArgs(self):
        """
        test the case that default arguments mixed with tuple arguments
        """
        def _method(arg1,defarg="default",*argtuple,**kwargs):
            return {"arg1":arg1,"default_arg":defarg,"arg_tuple":argtuple,"arg_dict":kwargs}
        
        self.assertEqual({"arg1":100,"arg_dict":{},"arg_tuple":(),"default_arg":"default"},_method(100))
        self.assertEqual({"arg1":101,"arg_dict":{},"arg_tuple":(),"default_arg":"newvalue"},_method(101,"newvalue"))
        self.assertEqual({"arg1":1,"arg_dict":{"x":99},"arg_tuple":(3,"cheka"),"default_arg":2},_method(1,2,3,"cheka",x=99))        
        
        # only when we have already filled out the two non-tuple argument, left argument will be collected into tuple
        self.assertEqual({"arg1":102,"arg_dict":{},"arg_tuple":(1,2),"default_arg":"newvalue2"},_method(102,"newvalue2",1,2))
        
        # keyword arguments are always the last
        self.assertEqual({"arg1":103,"arg_dict":{"x":99},"arg_tuple":(),"default_arg":"cheka"},
                         _method(arg1=103,defarg="cheka",x=99))