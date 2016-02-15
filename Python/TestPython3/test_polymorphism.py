
import unittest

# ------------------------------- Template Pattern
class TemplateBase(object):
    def templateMethod(self):
        return self.subMethod1() + self.subMethod2()
    
    def subMethod1(self):
        raise NotImplementedError("must override in derived class")
    
    def subMethod2(self):
        raise NotImplementedError("must override in derived class")
    
    def subMethod3(self):
        return 5
    
class DerivedForInt(TemplateBase):
    def subMethod1(self): return 1 
    def subMethod2(self): return 2 
    def subMethod3(self):
        return 2 * super(DerivedForInt,self).subMethod3() 
    
class DerivedForString(TemplateBase):
    def subMethod1(self): return "cheka"
    def subMethod2(self): return " stasi"
# ------------------------------- End Template Pattern
        
class TestPolymorphism(unittest.TestCase):
    # =================== helper class definition
    class Grandpa(object) : pass
    class Father(Grandpa) : pass
    class Son(Father) : pass
        
    def testTemplatePattern(self):
        int_product = DerivedForInt()
        self.assertEqual(3,int_product.templateMethod())
        
        string_product = DerivedForString()
        self.assertEqual("cheka stasi",string_product.templateMethod())
        
    def testCallSuperMethod(self):
        obj = DerivedForString()
        self.assertEqual(5,obj.subMethod3()) # original base method
        
        obj = DerivedForInt()
        self.assertEqual(10,obj.subMethod3()) # override base method, but call base version inside it
        
    def testIssubclass(self):
        self.assertTrue(issubclass(TestPolymorphism.Son,TestPolymorphism.Father))
        self.assertTrue(issubclass(TestPolymorphism.Father,TestPolymorphism.Grandpa))
        # can be used to judge not-direct inheritance
        self.assertTrue(issubclass(TestPolymorphism.Son,TestPolymorphism.Grandpa))
        
    def testIsinstance(self):
        agrandpa = TestPolymorphism.Grandpa()
        afather = TestPolymorphism.Father()
        ason = TestPolymorphism.Son()
        
        # can be used to test super class
        self.assertTrue(isinstance(ason,TestPolymorphism.Father))
        self.assertTrue(isinstance(ason,TestPolymorphism.Grandpa))
        self.assertTrue(isinstance(afather,TestPolymorphism.Grandpa))
        

if __name__ == "__main__":
    unittest.main()