using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LinePoints
{
    [SerializeField] List<Vector3> points;
    LinePoints()
    {
        points = new List<Vector3>();
    }
    LinePoints(List<Vector3> _points)
    {
        this.points = _points;
    }
    LinePoints(LinePoints original)
    {
        points = original.points;
    }
    public void SetPoints(List<Vector3> _points) { this.points = _points; }
    public List<Vector3> GetPoints() { return points; }

    public int GetCount() { return points.Count; }
}
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class UILineRenderer : Graphic
{
    public LinePoints LinePoints { get { return m_linePoints; } set { m_linePoints = value; UpdateMesh(); } }
    public Gradient Color { get { return m_color; } set { m_color = value; UpdateMesh(); } }
    public float Width { get { return m_width; } set { m_width = value; UpdateMesh(); } }
    public AnimationCurve WidthDistrib { get { return m_widthDistrib; } set { m_widthDistrib = value; UpdateMesh(); } }

    [SerializeField] private LinePoints m_linePoints;
    [SerializeField] private Gradient m_color;
    [SerializeField] private float m_width = 0.1f;
    [SerializeField] private AnimationCurve m_widthDistrib;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        /*���b�V���Ⓒ�_���������B�L���b�V���ł���Ȃ炢��Ȃ��B
          �p�t�H�[�}���X�I�ɂ��L���b�V���ł���Ȃ炵���ق����ǂ��B
          �������A���񐶐�����̂ł���Β��_���̏��(65000)�ɒB����̂ŕK�v�B*/
        vh.Clear();
        //���_����struct
        UIVertex v = UIVertex.simpleVert;
        v.color = Color.Evaluate(0);
        
        for (int i = 0; i < m_linePoints.GetCount(); i++)
        {
            //���̎n�_
            Vector2 start = CreatePos(m_linePoints.GetPoints().ToArray()[i]);
            //���̏I�_
            //�p�X�̏I���Ȃ�O��̔��Ε���
            Vector2 end = (i < m_linePoints.GetCount() - 1) ? CreatePos(m_linePoints.GetPoints().ToArray()[i + 1])
                                                            : start - (CreatePos(m_linePoints.GetPoints().ToArray()[i - 1]) - start);

            //���̑O��_
            //�p�X�̎n�܂�Ȃ玟��̔��Ε���
            Vector2 pre = (i >= 1) ? CreatePos(m_linePoints.GetPoints().ToArray()[i - 1])
                                    : start-(end - start);
            Vector2 relate = -(pre - start) + (end - start);
            Vector2 origin = start;


            for (int j = -1; j <= 1; j += 2)
            {
                Vector2 vertical = new Vector2(0, j);
                vertical = MyMath.ComplexMul(vertical, relate).normalized * (m_width / 2);
                v.position = origin + CreatePos(vertical);
                vh.AddVert(v);
            }
            if (i >= 1)
            {
                //vh.AddTriangle(i * 2 - 1, i * 2, i * 2 - 3);
                //vh.AddTriangle(i * 2, i * 2 - 2, i * 2 - 1);
            }

        }
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 1);

        List<UIVertex> stream = new List<UIVertex>();
        vh.GetUIVertexStream(stream);

        int vi = 0;
        string debugtext = "";
        foreach (var vert in stream)
        {
            debugtext += vi + ":" + vert.position + "\n";
            vi++;
        }
        Debug.Log(debugtext);
    }

    //���������̈ʒu�ϊ��֐��B����(0,0)�A�E��(1,1)�Ƃ��ē��͂���Ƃ��傤�ǂ悭�ϊ������B
    private Vector2 CreatePos(Vector2 pos)
    {
        Vector2 p = Vector2.zero;
        p.x -= rectTransform.pivot.x;
        p.y -= rectTransform.pivot.y;
        p.x += pos.x;
        p.y += pos.y;
        p.x *= rectTransform.rect.width;
        p.y *= rectTransform.rect.height;
        return p;
    }
    private void AddVert(VertexHelper vh, Vector2 pos)
    {
        var vert = UIVertex.simpleVert;
        vert.position = pos;
        vert.color = color;
        vh.AddVert(vert);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //SetVerticesDirty();
    }

    public void UpdateMesh()
    {
        SetVerticesDirty();
    }
}
