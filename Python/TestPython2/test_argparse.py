
import unittest
import argparse

class ArgparseTest(unittest.TestCase):
    
    def setUp(self):
        self._parser = argparse.ArgumentParser(description="unittest argparse module")
        
    def parse(self,commandline):
        return self._parser.parse_args(commandline.split())   
    
    def test_none_as_default(self):
        """
        if optional argument is not assigned, then its value will be None
        """
        self._parser.add_argument("-f","--foo",type=int,help="test default value")
        
        args = self.parse("")
        self.assertTrue(args.foo is None)
        
        with self.assertRaises(SystemExit):
            self.parse("-f")
        
        args = self.parse("-f 1")
        self.assertEqual(1,args.foo)
        
    def test_as_flag(self):
        """
        flag arguments. we can either pass nothing, or just pass the "-f" without extra value
        we just need a flag
        if pass that argument name, then that flag is set
        otherwise, the opposite of the flag is set
        """
        self._parser.add_argument("-f","--foo",action="store_true")
        
        # if not assigned, since we have "store_true" action
        # the default value will not be None any more
        # however, will be the opposite of "store_true", so the default value is False
        arg1 = self.parse("")
        self.assertTrue(arg1.foo is not None)
        self.assertFalse(arg1.foo)
        
        arg2 = self.parse("-f")
        self.assertTrue(arg2.foo)
        
        with self.assertRaises(SystemExit):
            # we cannot pass, nor need, any extra arguments
            self.parse("-f True")
    
    def test_simple_add_args(self):
        self._parser.add_argument("bar",type=int,help="positional arguments")
        self._parser.add_argument("-f","--foo",type=int,help="optional arguments")
        
        args = self.parse("99 -f 101")        
        self.assertEqual(99,args.bar)
        self.assertEqual(101,args.foo)
        
    def test_multiple_positional_args(self):
        self._parser.add_argument("n",type=int)
        self._parser.add_argument("s")
        
        args = self.parse("1 stasi")
        self.assertEqual(1,args.n)
        self.assertEqual("stasi",args.s)
        
    def test_action_store_const(self):
        self._parser.add_argument("-f1","--foo1",action="store_const",const=1)
        self._parser.add_argument("-f2","--foo2",action="store_const",const=2)
        
        args = self.parse("-f1 -f2")
        self.assertEqual(1,args.foo1)
        self.assertEqual(2,args.foo2)
        
        args = self.parse("-f1")
        self.assertEqual(1,args.foo1)
        self.assertTrue(args.foo2 is None)    
        
        args = self.parse("")
        self.assertTrue(args.foo1 is None)    
        self.assertTrue(args.foo2 is None)    
        
    
    def test_nargs_n(self):
        self._parser.add_argument("-f","--fool",nargs=3)
        self._parser.add_argument("bar",nargs=2)
        
        args = self.parse("a b -f c d e")
        self.assertEqual(["a","b"],args.bar)
        self.assertEqual(["c","d","e"],args.fool)
        
    def test_nargs_question_mark(self):
        """
        One argument will be consumed from the command line if possible, and produced as a single item. 
        If no command-line argument is present, the value from default will be produced. 
        Note that for optional arguments, there is an additional case - 
        the option string is present but not followed by a command-line argument. 
        In this case the value from const will be produced.
        """
        self._parser.add_argument('--foo', nargs='?', const='c', default='d')
        self._parser.add_argument('bar', nargs='?', default='d')
        
        args1 = self.parse("xx --foo yy")
        self.assertEqual("xx",args1.bar)
        self.assertEqual("yy",args1.foo)
        
        args2 = self.parse("xx --foo")
        self.assertEqual("xx",args2.bar)
        self.assertEqual("c",args2.foo) # taken from "const" other than "default"
        
        args3 = self.parse("")
        # both taken from "default"
        self.assertEqual("d",args3.bar)
        self.assertEqual("d",args3.foo) 
        
    def test_default_positional_arg(self):
        """
        For positional arguments with nargs equal to ? or *, the default value is used when no command-line argument was present:
        so default positional argument, we must specify "nargs"
        """
        self._parser.add_argument("bar",type=int,nargs="?",default=9)
        
        args = self.parse("100")
        self.assertEqual(100,args.bar)
        
        args = self.parse("")
        self.assertEqual(9,args.bar)
        
    def test_default_optional_arg(self):
        """
        For optional arguments, the default value is used when the option string was not present at the command line
        NOT the case when "only optional string, no argument"
        """
        self._parser.add_argument("-f","--foo",type=int,default=9)
        
        # ---------- pass explicit argument
        args = self.parse("-f 888")
        self.assertEqual(888,args.foo)
        
        # ---------- no optional string, no argument
        args = self.parse("")
        self.assertEqual(9,args.foo)
        
        # ---------- optional string, no argument
        # ---------- is not allowed
        with self.assertRaises(SystemExit):
            self.parse("-f")
        
    def test_const_default(self):
        # if you have "const=xxx", you must also include "nargs=?"
        # otherwise, it will throw an exception
        self._parser.add_argument("-f","--foo",type=int,nargs="?",const=1,default=9)
        
        # ---------- pass explicit argument
        args = self.parse("-f 888")
        self.assertEqual(888,args.foo)
        
        # ---------- no optional string, no argument
        # ---------- value comes from "default"
        args = self.parse("")
        self.assertEqual(9,args.foo)
        
        # ---------- optional string, no argument
        # ---------- value comes from "const"
        args = self.parse("-f")    
        self.assertEqual(1,args.foo)
            
        
    def test_nargs_star_mark(self):
        """
        All command-line arguments present are gathered into a list
        """
        self._parser.add_argument('--foo', nargs='*')
        self._parser.add_argument('--bar', nargs='*',type=int)
        self._parser.add_argument('baz', nargs='*')
        
        args = self.parse("a b --foo x y --bar 1 2")
        
        self.assertEqual(["a","b"],args.baz)
        self.assertEqual(["x","y"],args.foo)
        self.assertEqual([1,2],args.bar)
        
    def test_nargs_plus_mark(self):
        """
        Just like '*', all command-line args present are gathered into a list. 
        Additionally, an error message will be generated if there wasn't at least one command-line argument present. 
        """
        self._parser.add_argument('foo', nargs='+')
        
        args = self.parse("a b")
        self.assertEqual(["a","b"],args.foo)
        
        with self.assertRaises(SystemExit):
            args = self.parse("")
            
    def test_choices(self):
        self._parser.add_argument("direction",choices=("left","right"))
        self._parser.add_argument("-m","--move",type=int,choices=range(1,4))
        
        # ---------------------- normal
        args = self.parse("left -m 1")
        self.assertEqual("left",args.direction)
        self.assertEqual(1,args.move)
        
        # ---------------------- out of range
        with self.assertRaises(SystemExit):
            args = self.parse("up -m 2")
            
        with self.assertRaises(SystemExit):
            args = self.parse("left -m 100")
        