package wsu.cheka.basictest;

import java.io.Serializable;

/**
 * test object
 * @author cheka
 */
class StudentInfo implements Serializable,Cloneable 
{
	private String m_name;
	private int m_id;

	public StudentInfo() {
	}

	public StudentInfo(String name, int id) {
		m_name = name;
		m_id = id;
	}

	public String getName() {
		return m_name;
	}

	public void setName(String name) {
		m_name = name;
	}

	public int getID() {
		return m_id;
	}

	public void setID(int id) {
		m_id = id;
	}

	@Override
	public boolean equals(Object other) {
		if (other == null)
			return false;
		if (this == other)
			return true;
		if (!(other instanceof StudentInfo))
			return false;

		StudentInfo otherStudent = (StudentInfo) other;
		return (m_name.equals(otherStudent.m_name) && (m_id == otherStudent.m_id));
	}

	@Override
	public int hashCode() {
		String strhashcode = String.format("%s-%d", m_name, m_id);
		return strhashcode.hashCode();
	}
	
	public StudentInfo clone() throws CloneNotSupportedException
	{
		return (StudentInfo)super.clone();
	}
}