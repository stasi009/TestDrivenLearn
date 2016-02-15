
import unittest

class Shape(object):
    def __init__(self,name):
        self.name = name

class Circle(Shape):
    def __init__(self,name,r):
        Shape.__init__(self,name) # call base's constructor in old way
        self.r = r

class Rectangle(Shape):
    def __init__(self,name,height,width):
        super(Rectangle,self).__init__(name) # is the same as above codes
        self.height = height
        self.width = width

class Grandpa(object) : pass
class Father(Grandpa) : pass
class Son(Father) : pass

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
        
class InheritanceTest(unittest.TestCase):

    def test_constructors(self):
        # -------------------- old way
        c = Circle("circle",9)
        self.assertEqual(9,c.r)
        self.assertEqual("circle",c.name)

        # -------------------- new way
        r = Rectangle("rectangle",1,2)
        self.assertEqual("rectangle",r.name)
        self.assertEqual(1,r.height)
        self.assertEqual(2,r.width)
        
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
        self.assertTrue(issubclass(Son,Father))
        self.assertTrue(issubclass(Father,Grandpa))
        # can be used to judge not-direct inheritance
        self.assertTrue(issubclass(Son,Grandpa))
        
    def testIsinstance(self):
        agrandpa = Grandpa()
        afather = Father()
        ason = Son()
        
        # can be used to test super class
        self.assertTrue(isinstance(ason,Father))
        self.assertTrue(isinstance(ason,Grandpa))
        self.assertTrue(isinstance(afather,Grandpa))
        

if __name__ == "__main__":
    unittest.main()