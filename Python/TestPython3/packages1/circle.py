
########################################################################
class Circle(object):
    """circle class"""
    
    #--------------- static variables
    _all = []
    _pi = 3.14159
    
    #--------------- static methods
    @staticmethod
    def totalArea():
        total = 0
        for c in Circle._all:
            total += c.area()
        return total
    
    @staticmethod
    def totalCircles():
        return len(Circle._all)

    #--------------- member methods
    def __init__(self,r=1):
        self.radius = r
        self.__class__._all.append(self)
        
    def area(self):
        return self.__class__._pi * self.radius * self.radius
        
    
        
        
    
    