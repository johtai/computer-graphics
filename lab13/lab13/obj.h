//#ifndef _OBJ_H
//#define _OBJ_H
//#include "shapes.h"
//class CLoadObj 
//{
//public:
//
//	// Вы будете вызывать только эту функцию. Просто передаёте структуру
//	// модели для сохранения данных, и имя файла для загрузки.
//	bool ImportObj(Object3D* pModel, char* strFileName);
//
//	// Главный загружающий цикл, вызывающийся из ImportObj()
//	void ReadObjFile(Object3D* pModel);
//
//	// Вызывается в ReadObjFile() если линия начинается с ‘v’
//	void ReadVertexInfo();
//
//	// Вызывается в ReadObjFile() если линия начинается с ‘f’
//	void ReadFaceInfo();
//
//	// Вызывается после загрузки информации полигонов
//	void FillInObjectInfo(Object3D* pModel);
//
//	// Вычисление нормалей. Это не обязятельно, но очень желательно.
//	void ComputeNormals(Object3D* pModel);
//
//	// Так как .obj файлы не хранят имен текстур и информации о материалах, мы создадим
//	// функцию, устанавливающую их вручную. materialID — индекс для массива pMaterial нашей модели.
//	void SetObjectMaterial(Object3D* pModel, int whichObject, int materialID);
//
//	// Чтобы проще присваивать материал к .obj обьекту, создадим для этого функцию.
//	// Передаём в неё модель, имя материала, имя файла текстуры и цвет RGB.
//	// Если нам нужен только цвет, передаём NULL для strFile.
//	void AddMaterial(Object3D* pModel, char* strName, char* strFile,
//		int r = 255, int g = 255, int b = 255);
//
//private:
//
//	// Указатель на файл
//	FILE* FilePointer;
//
//	// STL vector, содержащий список вершин
//	std::vector<std::vector>  m_pVertices;
//
//	// STL vector, содержащий список полигонов
//	std::vector<tFace> m_pFaces;
//
//	// STL vector, содержащий список UV координат
//	std::vector<CVector2>  m_pTextureCoords;
//
//	// Говорит нам, имеет ли обьект текстурные координаты
//	bool m_bObjectHasUV;
//
//	// Говорит нам, что мы только что прочитали данные полигонов, чтобы мы могли читать несколько обьектов
//	bool m_bJustReadAFace;
//};
//
//#endif