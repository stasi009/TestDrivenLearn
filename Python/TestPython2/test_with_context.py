
import unittest
from contextlib import contextmanager

class MyLogger(object):
    Debug = 0
    Info = 1
    Warning = 2
    Alert = 3

    _all_loggers = {}

    def __init__(self,level):
        self.level = level
        self.logs = []

    @property
    def level(self): return self._level

    @level.setter
    def level(self,value): self._level = value

    def log(self,level,text):
        if level >= self._level :
            self.logs.append(text)

    def debug(self,text): self.log(MyLogger.Debug,text)
    def info(self,text): self.log(MyLogger.Info,text)
    def warning(self,text): self.log(MyLogger.Warning,text)
    def alert(self,text): self.log(MyLogger.Alert,text)

    # we cannot set the default value to one of the enumeration
    # because, at that time, the whole class definition hasn't completed yet
    @classmethod
    def get_logger(cls,name,optLevel = None):

        level = optLevel
        if level is None:
            level = MyLogger.Warning

        if name in cls._all_loggers:
            return cls._all_loggers[name]
        else:
            newlogger = MyLogger(level)
            cls._all_loggers[name] = newlogger
            return newlogger

   
    # the order is important, @classmethod must be the outer one
    @classmethod
    @contextmanager
    def temp_set_level(cls,name,level):
        logger = cls.get_logger(name)
        oldlevel = logger.level
        logger.level = level
        try:
            yield logger # The yield expression is the point at which the with block's contents will execute
        finally:
            logger.level = oldlevel


class WithContextTest(unittest.TestCase):

    def setUp(self):
        MyLogger._all_loggers.clear()
    
    def test_without_change_context(self):
        logger1 = MyLogger.get_logger("test")
        
        logger2 = MyLogger.get_logger("test")
        self.assertIs(logger1,logger2)
        self.assertEqual(MyLogger.Warning,logger2.level)

        logger1.debug("debug")
        logger1.alert("alert")
        logger1.info("info")
        self.assertEqual(["alert"],logger2.logs)

    def test_with_change_context(self):
        logger_name = "test"

        with MyLogger.temp_set_level(logger_name,MyLogger.Info) as logger:
            self.assertEqual(logger.level,MyLogger.Info)
            logger.info("info")
            logger.alert("alert")
            self.assertEqual(["info","alert"],logger.logs)
            del logger.logs[:]

        # outside the context, the old level is restored
        logger = MyLogger.get_logger(logger_name)
        self.assertEqual(MyLogger.Warning,logger.level)
        logger.info("info")
        logger.alert("alert")
        self.assertEqual(["alert"],logger.logs)


