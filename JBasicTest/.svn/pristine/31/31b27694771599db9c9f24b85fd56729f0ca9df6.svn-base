package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import java.io.File;

import org.junit.Test;

public class FileTest {
	private static final String WORKING_DIR = "D:\\study\\programming\\java\\JBasicTest";

	@Test
	public void testExist() {
		// check with relative path
		// !!!!!!!!!!!!! this test also shows that Eclipse run our codes
		// !!!!!!!!!!!!! taking the project directory as the working directory
		// (current working directory is always the directory invoking the JVM)
		// !!!!!!!!!!!!! (and always include the "bin" directory of current
		// working directory into its classpath)
		File relativeFile = new File("resources/testFileExists.txt");
		assertTrue(relativeFile.exists());

		// check absolute path
		File absoluteFile = new File("C:/Java/junit/junit-4.8.1-src.jar");
		assertTrue(absoluteFile.exists());
	}

	@Test
	public void testClassGetResource() {
		// check the resource file with relative path to the location of the
		// class
		File relativeResource = new File(FileTest.class.getResource(
				"tetsClassGetResource/tetsClassGetResource.txt").getFile());
		assertTrue(relativeResource.exists());

		// check the resource file with absolute path comparing to the root path
		// of the namespace
		File absoluteResourceFile = new File(
				FileTest.class
						.getResource(
								"/wsu/cheka/basictest/tetsClassGetResource/tetsClassGetResource.txt")
						.getFile());
		assertTrue(absoluteResourceFile.exists());

		// also access the resource with absolute path
		absoluteResourceFile = new File(FileTest.class.getResource(
				"/testResource.txt").getFile());
		assertTrue(absoluteResourceFile.exists());
	}

	/**
	 * class's getResource can have two method to access the file 1. using the
	 * relative path, which is relative to the location of the class file 2.
	 * using the absolute path, which always starts with "/", which is always
	 * relative to namespace's root path
	 * 
	 * different from class, ClassLoader just has only one method to access the
	 * location of resource that always relative to namespace's root path but
	 * without "/" at the beginning.
	 */
	@Test
	public void testClassLoaderGetResource() {
		// pay attention that no "/" at the beginning
		File absoluteFile1 = new File(FileTest.class.getClassLoader()
				.getResource("testResource.txt").getFile());
		assertTrue(absoluteFile1.exists());

		File absoluteFile2 = new File(FileTest.class.getClassLoader()
				.getResource("wsu/cheka/basictest/testEcliseCopy.txt")
				.getFile());
		assertTrue(absoluteFile2.exists());
	}

	@Test
	public void testWorkingDirectory() {
		assertEquals(WORKING_DIR, System.getProperty("user.dir"));
	}
}
