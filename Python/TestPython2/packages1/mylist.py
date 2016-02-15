
class MyList(object):
    """
    demonstrate how to write a wrapper to add extra function to built-in type
    """
    def __init__(self,atype):
        self._list = []
        self._type = atype
        
    def append(self,data):
        if isinstance(data,self._type):
            self._list.append(data)
        else:
            raise TypeError("this list only limited to type[%s]"%(self._type.__name__))
        
    def __str__(self): 
        return str(self._list)
    
    def __len__(self):
        return len(self._list)
    
    def toList(self):
        return self._list
    
    def __getattr__(self,attr):
        """
        by using "__getattr__", there is no need to add wrapper API for each inside API
        """
        return getattr(self._list,attr)