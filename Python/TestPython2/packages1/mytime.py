
class MyTime(object):
    def __init__(self,hour,minute):
        self._hour = hour
        self._minute = minute
        
    def __str__(self):
        return "%d:%d"%(self._hour,self._minute)
    
    def __add__(self,other):
        return self.__class__(self._hour + other._hour,self._minute + other._minute)
    
    def __iadd__(self,other):
        """in place add"""
        self._hour += other._hour
        self._minute += other._minute
        return self
    
    def __eq__(self,other):
        if other is None:
            return False
        elif self is other:
            return True
        elif isinstance(other,self.__class__):
            return self._hour == other._hour and self._minute == other._minute
        else:
            return False
        
    def __ne__(self,other):
        return not self == other
    
    def __hash__(self):
        """make the class hashable, and can be used as the key"""
        return hash((self._hour,self._minute))
    
    def __lt__(self,other):
        if self._hour < other._hour : return True
        elif self._hour == other._hour and self._minute < other._minute: return True
        else: return False
        
    def __le__(self,other):
        return (self == other) or (self < other)
    
    def __gt__(self,other):
        return not self <= other
    
    def __ge__(self,other):
        return not self < other
    
    def __iter__(self):
        def generator():
            yield self._hour
            yield self._minute
        return generator()
    
    # in Python 3, this method should be called "__bool__"
    def __nonzero__(self):
        """let this object can be used as boolean"""
        return bool(self._hour or self._minute)
    
    def __getattr__(self,attr):
        """this method is called only when attr is not found in current instance"""
        if attr == "second":
            return 0
        else:
            raise AttributeError("not recognized attribute: %s"%attr)
        
    def __getitem__(self,index):
        if index == 0:
            return self._hour
        elif index == 1:
            return self._minute
        else:
            raise IndexError("Valid Index Only 0 and 1")
        
    def __call__(self):
        """
        it can has argument or not, both are OK
        because Python doesn't support overload, method with same name but different arguments will still overwrite previous one
        """
        return self._hour,self._minute