
import unittest
import string

########################################################################
class StringTest(unittest.TestCase):
    """demonstrate the usage of Python String"""
    
    def testEquality(self):
        a = "hello stasi"
        b = "hello stasi"
        c = a[:]
        
        self.assertEqual(a,b) # check against content
        self.assertTrue(a == b)
        
        # because python caches small strings, 
        # thus stores both strings in the same location
        self.assertIs(a,b)      
        self.assertTrue(a is c) # still caches

    def testCreate(self):
        s1 = 'I am a single quoted string'
        s2 = "I am a double quoted string"
        s3 = """I am a triple quoted string"""
        
    def testConcatenate(self):
        s1 = "hello"
        s2 = " sifang"
        self.assertEqual("hello sifang",s1+s2)
        
    def testLength(self):
        self.assertEqual(5,len("stasi"))
    
    def testMultiply(self):
        s = "stasi"
        ds = s*2
        
        self.assertEqual("stasistasi",ds,"multiply strings to concatenate")
        self.assertTrue("stasistasi" == ds)
        
    def testUpperLower(self):
        oristring = "sTAsi"
        self.assertEqual("STASI",oristring.upper())
        self.assertEqual("stasi",oristring.lower())
        # return just copy, the original string is not modified
        self.assertEqual("sTAsi",oristring)
        
    def testFormat(self):
        ########################## demo1
        percent_formatter = lambda n1,n2: "%d*%d=%d"%(n1,n2,n1*n2)
        method_formatter = lambda n1,n2: "{0}-{1}={2}".format(n1,n2,n1-n2)
        
        self.assertEqual("8*9=72",percent_formatter(8,9))
        self.assertEqual("8-9=-1",method_formatter(8,9))
        
         ########################## demo2
        formated_string = "hello %s from %s for %d times"%("wsu","cheka",10)
        self.assertEqual("hello wsu from cheka for 10 times",formated_string)
        
        # new format string in Python2.6 and Python3.0
        formated_string = "{0} {1} in {2}".format("hello","wsu",2010)
        self.assertEqual("hello wsu in 2010",formated_string)        
        
    def testAccessByIndex(self):
        astring = "sifang"
        
        atuple = tuple(astring)
        for index in range(len(atuple)):
            self.assertEqual(atuple[index],astring[index])
        
        self.assertEqual("i",astring[1])
        self.assertEqual("n",astring[-2])
        
    def testSlice(self):
        astring = "stasi"
        self.assertEqual("s",astring[-2:-1])
        self.assertEqual("si",astring[-2:])
        self.assertEqual("as",astring[2:4])
        self.assertEqual("asi",astring[2:])
        self.assertEqual("si",astring[-2:])
        self.assertEqual("st",astring[:2])
        
        ### with interval
        self.assertEqual("sai",astring[::2])
        self.assertEqual("isats",astring[::-1]) # reverse order   
        self.assertEqual("ias",astring[::-2])
        
    def testIn(self):
        a = "hello stasi"
        self.assertTrue("tas" in a)
        self.assertFalse("x" in a)
        
    def testReplace(self):
        self.assertEqual("chxxa","cheka".replace("ek","xx"))
        
        # replace accepts a third argument, indicating how many subsitution will occur
        self.assertEqual("1 2 3","1x2x3".replace("x"," "))
        self.assertEqual("1 2x3","1x2x3".replace("x"," ",1))
        
    def testStrRepr(self):
        aobject = [1,"cheka",100.0]
        presentation = "[1, 'cheka', 100.0]"
        
        self.assertEqual(presentation,str(aobject))
        self.assertEqual(presentation,repr(aobject))
        self.assertEqual(aobject,eval(presentation))
        
    def testOrdChr(self):
        self.assertEqual(97,ord("a"))
        self.assertEqual("a",chr(97))
        
    def testCompare(self):
        # upper characters are always smaller than lower characters
        self.assertTrue("Z" < "a")
        self.assertTrue("abc" < "xyz")
        self.assertTrue("abc" > "XYZ")
        
    def testConstants(self):
        self.assertEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ",string.ascii_uppercase)
        self.assertEqual("abcdefghijklmnopqrstuvwxyz",string.ascii_lowercase)
        self.assertEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",string.ascii_letters)
        self.assertEqual("0123456789",string.digits)
        
    def testTemplate(self):
        greeting_template = string.Template("hello ${target} from ${source} for ${number} times")
        self.assertEqual("hello wsu from cheka for 4 times",greeting_template.substitute(target="wsu",source="cheka",number=4))
        
        # missing key "number"
        self.assertRaises(KeyError,lambda : greeting_template.substitute(target="wsu",source="cheka"))
        
        # safe_subsitute won't throw exception when key is missed
        self.assertEqual("hello wsu from cheka for ${number} times",greeting_template.safe_substitute(target="wsu",source="cheka"))
        
    def testJoin(self):
        names = ["cheka","stasi","kgb"]
        self.assertEqual("cheka,stasi,kgb",",".join(names))
        self.assertEqual("cheka stasi kgb"," ".join(names))
        self.assertEqual("chekastasikgb","".join(names))
        
    def testCompiletimeConcatenate(self):
        concatenated = "cheka""wsu""lucky"
        self.assertEqual("chekawsulucky",concatenated)
    
    def testRawString(self):
        self.assertEqual("\\n",r"\n")
        self.assertEqual("D:\\newdoc",r"D:\newdoc")# useful in writing path
        
    def testCount(self):
        astring = "chekacheka"
        self.assertEqual(2,astring.count("ch"))
        
    def testSplit(self):
        msg = "hello, world"
        self.assertEqual(["hello,","world"],msg.split()) # default seperator is whitespace
        self.assertEqual(["hello"," world"],msg.split(","))
        
    def testFindAndIndex(self):
        name = "cheka"
        self.assertTrue(name.endswith("ka"))
        self.assertTrue(name.startswith("che"))
        
        self.assertEqual(2,name.find("ek"))
        self.assertEqual(-1,name.find("xy")) # doesn't find
        
        # "index" works just like "find", but when not found, instead returning -1 as find, throw an exception
        self.assertEqual(1,name.index("he"))
        self.assertRaises(ValueError, lambda : name.index("xy") )
        
    def testStrip(self):
        """
        Returns a copy of the string with the leading (lstrip) and trailing (rstrip) whitespace removed. 
        strip removes both.
        strip can not only remove white spaces, but can also remove line mark
        """
        name = "  cheka  \n"
        self.assertEqual("cheka",name.strip())
        self.assertEqual("cheka  \n",name.lstrip())
        self.assertEqual("  cheka",name.rstrip())    
        
        
if __name__ == "__main__":
    unittest.main()
        
        
        
    
    