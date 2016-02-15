
import unittest

class BasicClassTest(unittest.TestCase):
    
    # ======================================================== #
    # helper classes and methods
    # ======================================================== #
    class SimpleClass(object):
        """simple class just for test purpose"""
        staticField = 0
        
        def __init__(self):
            self.instance_field = 0
            self._proteced_field = 1
            self.__private_field = 2
            
        def setPrivateField(self,value):
            self.__private_field = value
            
        def getPrivateField(self):
            return self.__private_field
        
    def setUp(self):
        # clear the effect of last run
        BasicClassTest.SimpleClass.staticField = 0
            
    # ======================================================== #
    # test case 
    # ======================================================== #
    def testMemberVariables(self):
        obj1 = BasicClassTest.SimpleClass()
        obj2 = BasicClassTest.SimpleClass()
        
        # ------------------- test static member variables
        # change one affect another
        newvalue = 100
        # from here, we can see that if you want to access internal class, you have to use
        # its full name. you cannot use that internal name diretly even though you are in the outer class yourself
        BasicClassTest.SimpleClass.staticField = newvalue
        
        # static field can be read by either class or instance
        self.assertEqual(newvalue,BasicClassTest.SimpleClass.staticField)
        self.assertEqual(newvalue,obj1.staticField)
        self.assertEqual(newvalue,obj2.staticField)
        
        # ------------------- test instance member variables
        # change one will not affect others
        obj1.instance_field = newvalue
        self.assertEqual(newvalue,obj1.instance_field)
        self.assertEqual(0,obj2.instance_field)
        
    def testStaticVariables(self):
        """
        this test shows that static variables can be read by instance, because if this member field cannot
        be found at the instance scope, it will find it in the upper scope, then find it in the class scope
        
        but static member field cannot be written by instance, which not changing the static field, but
        create a new member field in the instance scope, so it never change the common field, so will not affect other instance
        """
        obj1 = BasicClassTest.SimpleClass()
        
        newvalue = 100
        # a new name is created within the instance scope and shadow the static one
        obj1.staticField = newvalue 
        
        self.assertEqual(newvalue,obj1.staticField)
        self.assertEqual(0,BasicClassTest.SimpleClass.staticField)
        self.assertTrue(obj1.staticField is not BasicClassTest.SimpleClass.staticField)
        
        # will never effect following instance
        obj2 = BasicClassTest.SimpleClass()
        self.assertEqual(0,obj2.staticField)
        self.assertTrue(obj2.staticField is BasicClassTest.SimpleClass.staticField)
        
    def testIs(self):
        # since user-defined class are mutable type
        # it can be guaranteed that below name binding will always bind to different objects
        obj1 = BasicClassTest.SimpleClass()
        obj2 = BasicClassTest.SimpleClass()
        
        self.assertFalse(obj1 is obj2)
        self.assertTrue(obj1 is not obj2)
        
    def testAccessIndicator(self):
        obj = BasicClassTest.SimpleClass()
        
        # actually protected member field prefixed with "_" is still able to be accessed outside object
        self.assertEqual(1,obj._proteced_field) 
        
        # private member field prefixed with "__" is totally unvisible from outside world
        self.assertRaises(AttributeError,lambda : obj.__private_field)
        
    def testType(self):
        obj = BasicClassTest.SimpleClass()
        
        self.assertTrue(isinstance(obj,BasicClassTest.SimpleClass))
        # below codes is true, because it derives from object, is a new class, other than a classic class
        # when define a new class, is creating a new type
        self.assertTrue(type(obj) is BasicClassTest.SimpleClass)
        self.assertTrue(obj.__class__ is BasicClassTest.SimpleClass)
        
        self.assertTrue(isinstance(BasicClassTest,type))
        self.assertTrue(isinstance(BasicClassTest.SimpleClass,type))
        
    def testDefaultEqual(self):
        # since we doesn't give any parameter, obj's member field are just default values
        # so below two objects must have the same content
        obj1 = BasicClassTest.SimpleClass()
        obj2 = BasicClassTest.SimpleClass()
        
        # because no "__eq__" is override, so just checking equality between below two objects with identifier comparison
        # so the same result with "is"
        self.assertNotEqual(obj1,obj2)
        self.assertFalse(obj1 is obj2)
        
    def testNameShadow(self):
        """
        this test shows that:
        
        1. write static member field by "ClassName" will just set new value to the static member field, which 
        will affect all the instance of this class
        
        2. write static member field by "object instance", 
        !!! will not set new value to the static member field
        !!! but will generate a new instance member field in the scope of that object instance, then shadow the static member
        field with the same name. later on, modify this newly created instance member field will not affect others
        
        !!! this feature belongs to a more general rule: naming and binding
        !!! the definition of a class is a block
        !!! then "static members" belongs to outer scope
        !!! while "instance members" declared with "self" belongs to inner scope
        !!! if "self.staticField" is used for read, for example, appears at right side of "="
        !!! then since that "NAME" cannot be found within local scope("self"), then it will look up for it
        !!! at outer scope and find that name, which is that static member field
        !!! when "self.staticField" appears at the left side of "=", it will just create a new name
        !!! within local scope("self"), then shadows the outer same name
        """
        obj1 = BasicClassTest.SimpleClass()
        obj2 = BasicClassTest.SimpleClass()
        
        newval1 = 100
        obj1.staticField = newval1 # !!!!!!!!!!! create a new instance member field in the scope of this object instance
        self.assertTrue(obj1.staticField is not BasicClassTest.SimpleClass.staticField)
        self.assertTrue(obj2.staticField is BasicClassTest.SimpleClass.staticField)
        self.assertEqual(0,BasicClassTest.SimpleClass.staticField)
        
        # ------------------- change the static member field now will not affect obj1
        newval2 = 200
        BasicClassTest.SimpleClass.staticField = newval2
        self.assertEqual(newval2,obj2.staticField)
        self.assertNotEqual(newval2,obj1.staticField)
        self.assertEqual(newval1,obj1.staticField) # still using its own copy in its instance scope
        
    def testInvokeMemberMethod(self):
        """
        !!!!!!! test invoking method not only by "object.method"
        !!!!!!! but also by "class.method(object)"
        !!!!!!! the later mechanism to invoke a method has its own advantage on functional programming
        !!!!!!! that the method can be passed as an argument
        """
        aobj = BasicClassTest.SimpleClass()
        
        value = 100
        aobj.setPrivateField(value)
        
        # member methods can be invoked by "Class.method(instance)"
        retrieved_value = BasicClassTest.SimpleClass.getPrivateField(aobj)
        
        self.assertEqual(retrieved_value,value)
        
if __name__ == "__main__":
    unittest.main()