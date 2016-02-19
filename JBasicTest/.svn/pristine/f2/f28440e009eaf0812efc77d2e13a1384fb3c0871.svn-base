package wsu.cheka.basictest;

import static org.junit.Assert.*;

import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

import org.junit.Before;
import org.junit.Test;

public class MapTest {
	private String[] m_keys;
	private String[] m_values;
	private Map<String, String> m_map;

	@Before
	public void setUp() {
		m_keys = new String[] { "KGB", "STASI", "MSS" };
		m_values = new String[] { "Russia", "Germany", "China" };
		m_map = new HashMap<String, String>();

		for (int index = 0; index < m_keys.length; ++index) {
			m_map.put(m_keys[index], m_values[index]);
		}
		Arrays.sort(m_keys);
		Arrays.sort(m_values);
	}

	@Test
	public void testIterateValues() {
		Collection<String> valueCollection = m_map.values();
		for (String value : valueCollection) {
			assertTrue(Arrays.binarySearch(m_values, value) >= 0);
		}
	}

	@Test
	public void testIterateKeys() {
		Collection<String> keyCollection = m_map.keySet();
		for (String key : keyCollection) {
			assertTrue(Arrays.binarySearch(m_keys, key) >= 0);
		}
	}

	@Test
	public void testIterateEntries() {
		int count = 0;
		for (Map.Entry<String, String> pair : m_map.entrySet()) {
			++count;
			assertTrue(Arrays.binarySearch(m_keys, pair.getKey()) >= 0);
			assertTrue(Arrays.binarySearch(m_values, pair.getValue()) >= 0);
		}
		assertEquals(m_keys.length, count);
	}
	
	private void makeStudentMap(Map<String, StudentInfo> mapStudents)
	{
		StudentInfo stud = new StudentInfo("cheka",1);
		mapStudents.put(stud.getName(), stud);
		
		stud = new StudentInfo("henry",2);
		mapStudents.put(stud.getName(), stud);
	}
	
	@Test
	public void testEquals()
	{
		Map<String, StudentInfo> srcMap = new HashMap<String, StudentInfo>();
		makeStudentMap(srcMap);
		
		Map<String, StudentInfo> cpyMap = new HashMap<String, StudentInfo>();
		makeStudentMap(cpyMap);
		
		//************************* to ensure that each value item will not reference to the same value item
		Iterator<StudentInfo> srcIt = srcMap.values().iterator();
		Iterator<StudentInfo> cpyIt = cpyMap.values().iterator();
		
		while (srcIt.hasNext() && cpyIt.hasNext())
		{
			StudentInfo srcStud = srcIt.next();
			StudentInfo cpyStud = cpyIt.next();
			assertNotSame(srcStud,cpyStud);
		}
		
		//************************* check that map compare equality based on content, rather then address
		assertEquals(srcMap, cpyMap);
	}
	
	@Test
	public void testGetNonExisted()
	{
		assertNull(m_map.get("NonExisted"));
	}
	
	@Test
	public void testPut()
	{
		Map<String, String> mapStrings = new HashMap<String, String>();
		
		String key = "Russia";
		String oldValue = "Cheka";
		String newValue = "KGB";
		
		assertNull(mapStrings.put(key, oldValue));// no previous value
		assertEquals(oldValue, mapStrings.put(key, newValue));
	}
}
