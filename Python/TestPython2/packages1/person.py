

class Person(object):

    def __init__(self,ssn,name):
        self._ssn = ssn
        self._name = name
        
    def __eq__(self,rhs):
        if self is rhs:
            return True
        elif isinstance(rhs,Person):
            return (self._ssn == rhs._ssn) and (self._name == rhs._name)
        else:
            return False
        
    def __ne__(self,rhs):
        return not self == rhs

    def __str__(self):
        return "Person<%d,%s>"%(self._ssn,self._name)
        