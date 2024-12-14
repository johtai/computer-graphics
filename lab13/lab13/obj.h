//#ifndef _OBJ_H
//#define _OBJ_H
//#include "shapes.h"
//class CLoadObj 
//{
//public:
//
//	// �� ������ �������� ������ ��� �������. ������ �������� ���������
//	// ������ ��� ���������� ������, � ��� ����� ��� ��������.
//	bool ImportObj(Object3D* pModel, char* strFileName);
//
//	// ������� ����������� ����, ������������ �� ImportObj()
//	void ReadObjFile(Object3D* pModel);
//
//	// ���������� � ReadObjFile() ���� ����� ���������� � �v�
//	void ReadVertexInfo();
//
//	// ���������� � ReadObjFile() ���� ����� ���������� � �f�
//	void ReadFaceInfo();
//
//	// ���������� ����� �������� ���������� ���������
//	void FillInObjectInfo(Object3D* pModel);
//
//	// ���������� ��������. ��� �� �����������, �� ����� ����������.
//	void ComputeNormals(Object3D* pModel);
//
//	// ��� ��� .obj ����� �� ������ ���� ������� � ���������� � ����������, �� ��������
//	// �������, ��������������� �� �������. materialID � ������ ��� ������� pMaterial ����� ������.
//	void SetObjectMaterial(Object3D* pModel, int whichObject, int materialID);
//
//	// ����� ����� ����������� �������� � .obj �������, �������� ��� ����� �������.
//	// ������� � �� ������, ��� ���������, ��� ����� �������� � ���� RGB.
//	// ���� ��� ����� ������ ����, ������� NULL ��� strFile.
//	void AddMaterial(Object3D* pModel, char* strName, char* strFile,
//		int r = 255, int g = 255, int b = 255);
//
//private:
//
//	// ��������� �� ����
//	FILE* FilePointer;
//
//	// STL vector, ���������� ������ ������
//	std::vector<std::vector>  m_pVertices;
//
//	// STL vector, ���������� ������ ���������
//	std::vector<tFace> m_pFaces;
//
//	// STL vector, ���������� ������ UV ���������
//	std::vector<CVector2>  m_pTextureCoords;
//
//	// ������� ���, ����� �� ������ ���������� ����������
//	bool m_bObjectHasUV;
//
//	// ������� ���, ��� �� ������ ��� ��������� ������ ���������, ����� �� ����� ������ ��������� ��������
//	bool m_bJustReadAFace;
//};
//
//#endif