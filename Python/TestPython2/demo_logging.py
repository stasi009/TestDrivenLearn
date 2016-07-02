
import logging

def default_level_is_warning():
    logging.warning('Watch out!')  # will print a message to the console
    logging.info('I told you so')  # will not print anything

def change_base_configuration():
    """
    basicConfig() should come before any calls to debug(), info() etc. 
    it¡¯s one-off, only the first call will actually do anything: subsequent calls are effectively no-ops
    """
    # below log file will be in 'append' mode
    # if want fresh start each time, set filemode to 'w'
    # logging.basicConfig(filename='example.log', filemode='w', level=logging.DEBUG)
    logging.basicConfig(filename='example.log',level=logging.DEBUG)# append to file
    logging.debug('This message should go to the log file')
    logging.info('So should this')
    logging.warning('And this, too')

def logging_variable_values():
    logging.warning('%s before you %s', 'Look', 'leap!')

def change_display_format():
    """
    default display format is: <severity>:<logger name>:<message>
    below example removes 'root' from display, result is: WARNING:And this, too
    """
    logging.basicConfig(format='%(levelname)s:%(message)s', level=logging.DEBUG)
    logging.debug('This message should appear on the console')
    logging.info('So should this')
    logging.warning('And this, too')

def log_with_datetime():
    logging.basicConfig(format='%(asctime)s %(message)s', datefmt='%m/%d/%Y %I:%M:%S %p')
    logging.warning('is when this event was logged.')


if __name__ == "__main__":
    default_level_is_warning()
