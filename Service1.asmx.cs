using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Services;
using Oracle.DataAccess.Client;

namespace is2maintenance
{
	/// <summary>
	/// [is2maintenance]
	/// </summary>
	//-------------------------------------------------------------------------------------
	// �C������
	//-------------------------------------------------------------------------------------
	// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j��
	//	disposeReader(reader);
	//	reader = null;
	// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
	//	logFileOpen(sUser);
	//	userCheck2(conn2, sUser);
	//	logFileClose();
	// MOD 2007.08.29 ���s�j���� �ғ����W�v�̕ύX
	//	���R�ʉ^���͕ʂŏW�v���ĉ��Z����
	// MOD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ�
	//	�f���@�������@�\�̒ǉ�
	// MOD 2007.10.22 ���s�j���� �^���ɒ��p�������Z�\��
	// ADD 2007.11.14 KCL) �X�{ global�Ή��̉���}�X�^�ꗗ�擾��ǉ�
	// ADD 2007.11.14 KCL) �X�{ global�Ή��̉�������f�[�^�擾��ǉ�
	// ADD 2007.11.14 KCL) �X�{ global�Ή��̂��˗���ꗗ�擾��ǉ�
	// ADD 2007.11.22 ���s�j���� �ꗗ���ڂɔ��X�b�c��\��
	//-------------------------------------------------------------------------------------
	// ADD 2008.03.21 ���s�j�O���[�o���Ή�
	// ADD 2008.02.14 ���s�j���� �Z�b�V�������̎擾
	// ADD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� 
	// ADD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� 
	// MOD 2008.11.26 ���s�j���� ���ۃR�[�h���󔒂ł��G���[���łȂ����� 
	// MOD 2008.12.01 ���s�j���� �o�׏Ɖ�̈ꗗ�̃\�[�g���̒��� 
	// DEL 2008.12.03 ���s�j���� �ב��l���݃`�F�b�N���畔��b�c�̂��΂���͂��� 
	// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� 
	//-------------------------------------------------------------------------------------
	// ADD 2009.01.06 ���s�j���� �p�X���[�h�`�F�b�N�Ή� 
	// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� 
	// ADD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j 
	// ADD 2009.04.02 ���s�j���� �ғ����Ή� 
	// MOD 2009.05.11 ���s�j���� ���o�בΉ� 
	// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� 
	// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX 
	// MOD 2009.07.09 ���s�j���� �z����񌟍��@�\�̒ǉ� 
	// MOD 2009.09.11 ���s�j���� �o�׏Ɖ�ŏo�׍ςe�f,���M�ςe�f�Ȃǂ�ǉ� 
	// MOD 2009.11.16 ���s�j���� �W��X���ꗗ�ɒǉ� 
	// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� 
	//-------------------------------------------------------------------------------------
	// MOD 2010.04.06 ���s�j���� �o�׏Ɖ�ɓ��Ӑ�A�X�֔ԍ��Ȃǂ�ǉ� 
	// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� 
	// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� 
	// MOD 2010.04.30 ���s�j���� �b�r�u�o�͋@�\�̒ǉ� 
	//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� 
	// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� 
	//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� 
	// MOD 2010.11.19 ���s�j���� �z�����Ȃǂ̎擾 
	// MOD 2010.11.25 ���s�j���� �o�׏Ɖ�ɍ폜�����Ȃǂ�ǉ� 
	// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� 
	//-------------------------------------------------------------------------------------
	// MOD 2011.02.02 ���s�j���� �o�׃f�[�^�o�ד��͈͎擾 
	// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� 
	// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� 
	// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� 
	// MOD 2011.05.31 ���s�j���� ���q�^���̑Ή� 
	//-------------------------------------------------------------------------------------
	// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ�
	// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ�
	//-------------------------------------------------------------------------------------
	// ADD 2014.09.10 BEVAS)�O�c�@�x�X�~�ߋ@�\�Ή�
	//-------------------------------------------------------------------------------------
	// MOD 2015.10.09 bevas�j���{ ���X�d�����R�[�h�o�^��ʂɎ��X���Ɠ����e���g���X��\��
	//-------------------------------------------------------------------------------------
	// ADD 2015.11.24 bevas�j���{ �o�׎��ѕ\�E�o�׃��x���C���[�W����@�\�ǉ�(is-2�Ǘ�)
	//-------------------------------------------------------------------------------------
	// MOD 2015.12.15 bevas) ���{ �A���֎~�G���A�@�\�Ή�(is-2�Ǘ��F���x���C���[�W�����)
	//-------------------------------------------------------------------------------------
	// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή�
	//                            �������Ĉȉ��̑Ή������{
	//                                �����x���C���[�W�̎Q�ƃe�[�u���g��
	//                                �����q�^���̉ғ����W�v�𒊏o�����ɒǉ��i�����䐔�֘A�j
	//-------------------------------------------------------------------------------------

	[System.Web.Services.WebService(
		 Namespace="http://Walkthrough/XmlWebServices/",
		 Description="is2maintenance")]

	public class Service1 : is2common.CommService
	{
		private static string sCRLF = "\\r\\n";
		private static string sSepa = "|";
		private static string sKanma = ",";
		private static string sDbl = "\"";
		private static string sSng = "'";

		public Service1()
		{
			//CODEGEN: ���̌Ăяo���́AASP.NET Web �T�[�r�X �f�U�C�i�ŕK�v�ł��B
			InitializeComponent();

			connectService();
		}

		#region �R���|�[�l���g �f�U�C�i�Ő������ꂽ�R�[�h 
		
		//Web �T�[�r�X �f�U�C�i�ŕK�v�ł��B
		private IContainer components = null;
				
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		/*********************************************************************
		 * ����}�X�^�擾
		 * �����F����b�c
		 * �ߒl�F�X�e�[�^�X�A����b�c�A������A�g�p�J�n���A�Ǘ��ҋ敪�A�g�p�I����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�����J�n");

			OracleConnection conn2 = null;
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
//			string[] sRet = new string[7];
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
//			string[] sRet = new string[8];
			string[] sRet = new string[9];
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT ����b�c "
					+       ",����� "
					+       ",�g�p�J�n�� "
					+       ",�Ǘ��ҋ敪 "
					+       ",�g�p�I���� "
					+       ",�X�V���� \n"
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
					+       ",�L���A�g�e�f \n"
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
					+       ",�ۗ�����e�f \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
					+  " FROM �b�l�O�P��� \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+    "AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetDecimal(5).ToString().Trim();
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
					sRet[7] = reader.GetString(6);
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
					sRet[8] = reader.GetString(7);
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ꗗ�擾
		 * �����F����b�c�A�����
		 * �ߒl�F�X�e�[�^�X�A����b�c�A�����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(����b�c) || '|' "
					+     "|| TRIM(�����) || '|' \n"
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
					+     "|| TRIM(�g�p�I����) || '|' "
					+     "|| TO_CHAR(SYSDATE,'YYYYMMDD') || '|' \n"
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END
					+  " FROM �b�l�O�P��� \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE ����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE ����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
// MOD 2006.06.28 ���s�j���� ���Ԉ�v�ɏC�� START
//					cmdQuery += " AND ����� LIKE '" + sKey[1] + "%' \n";
					cmdQuery += " AND ����� LIKE '%" + sKey[1] + "%' \n";
// MOD 2006.06.28 ���s�j���� ���Ԉ�v�ɏC�� END
				}
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND �g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END
				cmdQuery += " AND �폜�e�f = '0' \n";
				cmdQuery += " ORDER BY ����b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ǉ�
		 * �����F����b�c�A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�ǉ��J�n");
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			string s�ۗ�����e�f = (sKey.Length > 7) ? sKey[7] : "0";
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END

			OracleConnection conn2 = null;
			string[] sRet = new string[2];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f "
					+   "FROM �b�l�O�P��� "
					+  "WHERE ����b�c = '" + sKey[0] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				while (reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �b�l�O�P��� \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'" + sKey[3] + "' "
						+         ",'" + sKey[4] + "' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",' ' "
						+         ", 0 "
						+         ", 0 "
						+         ", 0 "
						+         ", 0 "
						+         ", 0 "
						+         ",'0' "
						+         "," + s�X�V����
						+         ",'����o�^' "
						+         ",'" + sKey[6] + "' "
						+         "," + s�X�V����
						+         ",'����o�^' "
						+         ",'" + sKey[6] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);

					tran.Commit();
					sRet[0] = "����I��";
					sRet[1] = s�X�V����;
				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �b�l�O�P��� \n"
							+   " SET ����� = '" + sKey[1] + "' "
							+       ",�g�p�J�n�� = '" + sKey[2] + "' "
							+       ",�g�p�I���� = '" + sKey[3] + "' "
							+       ",�Ǘ��ҋ敪 = '" + sKey[4] + "' "
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
							+       ",�L���A�g�e�f = '0' \n"
							+       ",�ۗ�����e�f = '" + s�ۗ�����e�f + "' \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
							+       ",�폜�e�f = '0' \n"
							+       ",�o�^���� = " + s�X�V����
							+       ",�o�^�o�f = '����o�^' "
							+       ",�o�^�� = '" + sKey[6] + "' "
							+       ",�X�V���� = " + s�X�V����
							+       ",�X�V�o�f = '����o�^' "
							+       ",�X�V�� = '" + sKey[6] + "' \n"
							+ " WHERE ����b�c = '" + sKey[0] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);

						tran.Commit();
						sRet[0] = "����I��";
						sRet[1] = s�X�V����;
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�X�V
		 * �����F����b�c�A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�X�V�J�n");
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			string s�ۗ�����e�f = (sKey.Length > 7) ? sKey[7] : "0";
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END

			OracleConnection conn2 = null;
			string[] sRet = new string[2];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�P��� \n"
					+   " SET ����� = '" + sKey[1] + "' "
					+       ",�g�p�J�n�� = '" + sKey[2] + "' " 
					+       ",�g�p�I���� = '" + sKey[3] + "' "
					+       ",�Ǘ��ҋ敪 = '" + sKey[4] + "' "
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
					+       ",�ۗ�����e�f = '" + s�ۗ�����e�f + "' "
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
					+       ",�X�V���� = " + s�X�V����
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[6] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� = " + sKey[5] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
					sRet[1] = s�X�V����;
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�폜
		 * �����F����b�c
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�폜�J�n");
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� START
			logWriter(sUser, INF, "���q�l�폜 ["+sKey[0]+"]");
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}

// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
			//�폜�Ώۉ�����Г��`����̏ꍇ�́A�b�l�O�T������X�e�e�[�u���̍폜�������Ď��s
			if(sKey[0].Substring(0,2).ToUpper() == "FK")
			{
				//�b�l�O�T�e�̍폜�ΏۃL�[���擾
				string[] sRet2 = new string[1];
				sRet2 = this.Sel_HouseSlipMember(sUser, sKey);
				if(!sRet2[0].Equals("����I��"))
				{
					sRet[0] = "�y�Г��`������X�폜�G���[�z" + sRet2[0];
					return sRet;
				}

				//�b�l�O�T�e���폜
				string[] sKey2 = new string[3];
				sKey2[0] = sKey[0];
				sKey2[1] = sRet2[5].Trim();
				sKey2[2] = sKey[2];
				string[] sRet3 = new string[1];
				sRet3 = this.Del_HouseSlipMember(sUser, sKey2);
				if(!sRet3[0].Equals("����I��"))
				{
					sRet[0] = "�y�Г��`������X�폜�G���[�z" + sRet3[0];
					return sRet;
				}
			}
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�P��� \n"
					+    "SET �폜�e�f = '1' "
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[2] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' "
					+   " AND �X�V���� = " + sKey[1] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^����
		 * �����F����b�c�A����b�c
		 * �ߒl�F�X�e�[�^�X�A����b�c�A���喼�A�o�͏��A�X�����A�X�V����
		 *
		 * �Q�ƌ��F����}�X�^.cs 2��
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�����J�n");

			OracleConnection conn2 = null;
// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//			string[] sRet = new string[10];
			string[] sRet = new string[19];
// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM02.����b�c "
					+      ", CM02.���喼 "
					+      ", CM02.�o�͏� "
					+      ", CM02.�X�֔ԍ� "
					+      ", NVL(CM10.�X����, ' ') "
					+      ", CM02.�ݒu��Z���P "
					+      ", CM02.�ݒu��Z���Q "
					+      ", CM02.�X�V���� \n"
					+      ", CM02.�T�[�}���䐔 \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//					+      ", CM02.�V���A���ԍ��P \n"
//					+      ", CM02.��ԂP \n"
//					+      ", CM02.�V���A���ԍ��Q \n"
//					+      ", CM02.��ԂQ \n"
//					+      ", CM02.�V���A���ԍ��R \n"
//					+      ", CM02.��ԂR \n"
//					+      ", CM02.�V���A���ԍ��S \n"
//					+      ", CM02.��ԂS \n"
//					+      ", CM02.�g�p�� \n"
//// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
					+      ", NVL(CM06.�V���A���ԍ��P,' ') \n"
					+      ", NVL(CM06.��ԂP,' ') \n"
					+      ", NVL(CM06.�V���A���ԍ��Q,' ') \n"
					+      ", NVL(CM06.��ԂQ,' ') \n"
					+      ", NVL(CM06.�V���A���ԍ��R,' ') \n"
					+      ", NVL(CM06.��ԂR,' ') \n"
					+      ", NVL(CM06.�V���A���ԍ��S,' ') \n"
					+      ", NVL(CM06.��ԂS,' ') \n"
					+      ", NVL(CM06.�g�p��,0) \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					+  " FROM �b�l�O�Q���� CM02 \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
					+      " LEFT JOIN �b�l�O�U����g�� CM06 \n"
					+      " ON CM02.����b�c = CM06.����b�c \n"
					+      " AND CM02.����b�c = CM06.����b�c \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX START
//					+  " LEFT JOIN �b�l�P�R�Z�� CM13 \n"
//					+    " ON CM02.�X�֔ԍ� = CM13.�X�֔ԍ� "
//					+   " AND CM13.�폜�e�f = '0' \n"
//					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
//					+    " ON CM13.�X���b�c = CM10.�X���b�c "
//					+   " AND CM10.�폜�e�f = '0' \n"
					+  " LEFT JOIN �b�l�P�S�X�֔ԍ� CM14 \n"
					+    " ON CM02.�X�֔ԍ� = CM14.�X�֔ԍ� "
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+   " AND CM14.�폜�e�f = '0' \n"
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
					+    " ON CM14.�X���b�c = CM10.�X���b�c "
					+   " AND CM10.�폜�e�f = '0' \n"
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX END
					+ " WHERE CM02.����b�c = '" + sKey[0] + "' \n"
					+   " AND CM02.����b�c = '" + sKey[1] + "' \n"
					+   " AND CM02.�폜�e�f = '0' \n"
					;

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetDecimal(2).ToString().Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetString(6).Trim();
					sRet[8] = reader.GetDecimal(7).ToString().Trim();
					sRet[9] = reader.GetDecimal(8).ToString().Trim();
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
					sRet[10] = reader.GetString(9).Trim();
					sRet[11] = reader.GetString(10).Trim();
					sRet[12] = reader.GetString(11).Trim();
					sRet[13] = reader.GetString(12).Trim();
					sRet[14] = reader.GetString(13).Trim();
					sRet[15] = reader.GetString(14).Trim();
					sRet[16] = reader.GetString(15).Trim();
					sRet[17] = reader.GetString(16).Trim();
					sRet[18] = reader.GetDecimal(17).ToString().Trim();
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
					iCnt++;
				}
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
				if(sRet[11].Trim().Length == 0) sRet[11] = "0"; 
				if(sRet[13].Trim().Length == 0) sRet[13] = "0"; 
				if(sRet[15].Trim().Length == 0) sRet[15] = "0"; 
				if(sRet[17].Trim().Length == 0) sRet[17] = "0"; 
				if(sRet[18].Trim().Length == 0) sRet[18] = "0"; 
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^��������
		 * �����F����b�c�A�X�֔ԍ�
		 * �ߒl�F�X�e�[�^�X�A����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_SectionCount(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���吔�����J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT COUNT(����b�c) "
//					= "SELECT NVL(COUNT(����b�c),0) "
					+   "FROM �b�l�O�Q���� CM02 "
					+  "WHERE ����b�c = '" + sKey[0] + "' "
					+    "AND �X�֔ԍ� = '" + sKey[1] + "' "
					+    "AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetDecimal(0).ToString().Trim();
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ꗗ�擾
		 * �����F����b�c�A����b�c�A���喼
		 * �ߒl�F�X�e�[�^�X�A����b�c�A���喼�A�o�͏��A�X�֔ԍ�
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(����b�c) || '|' "
					+     "|| TRIM(���喼) || '|' "
					+     "|| TRIM(�o�͏�) || '|' "
					+     "|| TRIM(�X�֔ԍ�) || '|' \n"
					+  " FROM �b�l�O�Q���� \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n";
				if (sKey[1].Trim().Length == 4)
				{
					cmdQuery += " AND ����b�c = '" + sKey[1] + "' \n";
				}
				else
				{
					cmdQuery += " AND ����b�c LIKE '" + sKey[1] + "%' \n";
				}
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND ���喼 LIKE '" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND �폜�e�f = '0' \n";
				cmdQuery += " ORDER BY ����b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ǉ�
		 * �����F����b�c�A����b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�ǉ��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f "
					+   "FROM �b�l�O�Q���� "
					+  "WHERE ����b�c = '" + sKey[0] + "' "
					+    "AND ����b�c = '" + sKey[1] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				while (reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �b�l�O�Q���� \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//						+         "(����b�c \n"
//						+         ",����b�c \n"
//						+         ",���喼 \n"
//						+         ",�g�D�b�c \n"
//						+         ",�o�͏� \n"
//						+         ",�X�֔ԍ� \n"
//						+         ",\"�W���[�i���m�n�o�^��\" \n"
//						+         ",\"�W���[�i���m�n�Ǘ�\" \n"
//						+         ",���^�m�n \n"
//						+         ",�o�ד� \n"
//						+         ",�ݒu��Z���P \n"
//						+         ",�ݒu��Z���Q \n"
//						+         ",�T�[�}���䐔 \n"
//						;
//						if(sKey.Length > 10){
//							cmdQuery = cmdQuery
//							+     ",�V���A���ԍ��P \n"
//							+     ",��ԂP \n"
//							+     ",�V���A���ԍ��Q \n"
//							+     ",��ԂQ \n"
//							+     ",�V���A���ԍ��R \n"
//							+     ",��ԂR \n"
//							+     ",�V���A���ԍ��S \n"
//							+     ",��ԂS \n"
//							+     ",�g�p�� \n"
//							;
//						}
//					cmdQuery = cmdQuery
//						+         ",�폜�e�f \n"
//						+         ",�o�^���� \n"
//						+         ",�o�^�o�f \n"
//						+         ",�o�^�� \n"
//						+         ",�X�V���� \n"
//						+         ",�X�V�o�f \n"
//						+         ",�X�V�� \n"
//						+         ") \n"
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",' ' "
						+         ", " + sKey[3]
						+         ",'" + sKey[4] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDD') " // �W���[�i���m�n�o�^��
						+         ", 0 "							 // �W���[�i���Ǘ��m�n
						+         ", 0 "							 // ���^�m�n
						+         ",TO_CHAR(SYSDATE,'YYYYMMDD') " // �o�ד�
						+         ",'" + sKey[5] + "' "
						+         ",'" + sKey[6] + "' "
						+         ", " + sKey[9] + "  "
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//						;
//						if(sKey.Length > 10){
//							cmdQuery = cmdQuery
//							+     ",'" + sKey[10] + "' "
//							+     ",'" + sKey[11] + "' "
//							+     ",'" + sKey[12] + "' "
//							+     ",'" + sKey[13] + "' "
//							+     ",'" + sKey[14] + "' "
//							+     ",'" + sKey[15] + "' "
//							+     ",'" + sKey[16] + "' "
//							+     ",'" + sKey[17] + "' "
//							+     ", " + sKey[18] + "  "
//							;
//						}
//					cmdQuery = cmdQuery
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
						+         ",'0' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'����o�^' "
						+         ",'" + sKey[8] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'����o�^' "
						+         ",'" + sKey[8] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
					if(sKey.Length > 10){
						cmdQuery
							= "INSERT INTO �b�l�O�U����g�� \n"
							+         "(����b�c \n"
							+         ",����b�c \n"
							+     ",�V���A���ԍ��P \n"
							+     ",��ԂP \n"
							+     ",�V���A���ԍ��Q \n"
							+     ",��ԂQ \n"
							+     ",�V���A���ԍ��R \n"
							+     ",��ԂR \n"
							+     ",�V���A���ԍ��S \n"
							+     ",��ԂS \n"
							+     ",�g�p�� \n"
							+         ",�폜�e�f \n"
							+         ",�o�^���� \n"
							+         ",�o�^�o�f \n"
							+         ",�o�^�� \n"
							+         ",�X�V���� \n"
							+         ",�X�V�o�f \n"
							+         ",�X�V�� \n"
							+         ") \n"
							+ " VALUES ('" + sKey[0] + "' " 
							+         ",'" + sKey[1] + "' "
							+     ",'" + sKey[10] + "' "
							+     ",'" + sKey[11] + "' "
							+     ",'" + sKey[12] + "' "
							+     ",'" + sKey[13] + "' "
							+     ",'" + sKey[14] + "' "
							+     ",'" + sKey[15] + "' "
							+     ",'" + sKey[16] + "' "
							+     ",'" + sKey[17] + "' "
							+     ", " + sKey[18] + "  "
							+         ",'0' "
							+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+         ",'����o�^' "
							+         ",'" + sKey[8] + "' "
							+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+         ",'����o�^' "
							+         ",'" + sKey[8] + "' "
							+ " ) \n";

						CmdUpdate(sUser, conn2, cmdQuery);
					}
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					sRet[0] = "����I��";
				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �b�l�O�Q���� \n"
							+   " SET ���喼 = '" + sKey[2] + "' "
							+       ",�g�D�b�c = ' ' "
							+       ",�o�͏� = " + sKey[3] 
							+       ",�X�֔ԍ� = '" + sKey[4] + "' "
							+       ",�W���[�i���m�n�o�^�� = TO_CHAR(SYSDATE,'YYYYMMDD') "
							+       ",�W���[�i���m�n�Ǘ� = 0 "
							+       ",���^�m�n = 0 "
							+       ",�o�ד� = TO_CHAR(SYSDATE,'YYYYMMDD') "
							+       ",�ݒu��Z���P = '" + sKey[5] + "' "
							+       ",�ݒu��Z���Q = '" + sKey[6] + "' "
							+       ",�T�[�}���䐔 =  " + sKey[9] + "  "
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//							;
//							if(sKey.Length > 10){
//								cmdQuery = cmdQuery
//								+     ",�V���A���ԍ��P = '" + sKey[10] + "' \n"
//								+     ",��ԂP = '" + sKey[11] + "' \n"
//								+     ",�V���A���ԍ��Q = '" + sKey[12] + "' \n"
//								+     ",��ԂQ = '" + sKey[13] + "' \n"
//								+     ",�V���A���ԍ��R = '" + sKey[14] + "' \n"
//								+     ",��ԂR = '" + sKey[15] + "' \n"
//								+     ",�V���A���ԍ��S = '" + sKey[16] + "' \n"
//								+     ",��ԂS = '" + sKey[17] + "' \n"
//								+     ",�g�p�� = " + sKey[18] + "  \n"
//								+     ",����\���Ǘ��ԍ� = 0 \n"
//								+     ",�׎�l�J�z���@ = ' ' \n"
//								+     ",�׎�l�J�z�N�� = ' ' \n"
//								+     ",�t���O�P = ' ' \n"
//								+     ",�t���O�Q = ' ' \n"
//								+     ",�t���O�R = ' ' \n"
//								;
//							}
//						cmdQuery = cmdQuery
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
							+       ",�폜�e�f = '0' "
							+       ",�o�^���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�o�^�o�f = '����o�^' "
							+       ",�o�^�� = '" + sKey[8] + "' "
							+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�X�V�o�f = '����o�^' \n"
							+       ",�X�V�� = '" + sKey[8] + "'"
							+ " WHERE ����b�c = '" + sKey[0] + "' \n"
							+   " AND ����b�c = '" + sKey[1] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
						if(sKey.Length > 10){
							cmdQuery
								= "UPDATE �b�l�O�U����g�� SET \n"
							+     " �V���A���ԍ��P = '" + sKey[10] + "' \n"
							+     ",��ԂP = '" + sKey[11] + "' \n"
							+     ",�V���A���ԍ��Q = '" + sKey[12] + "' \n"
							+     ",��ԂQ = '" + sKey[13] + "' \n"
							+     ",�V���A���ԍ��R = '" + sKey[14] + "' \n"
							+     ",��ԂR = '" + sKey[15] + "' \n"
							+     ",�V���A���ԍ��S = '" + sKey[16] + "' \n"
							+     ",��ԂS = '" + sKey[17] + "' \n"
							+     ",�g�p�� = " + sKey[18] + "  \n"
							+     ",����\���Ǘ��ԍ� = 0 \n"
							+     ",�׎�l�J�z���@ = ' ' \n"
							+     ",�׎�l�J�z�N�� = ' ' \n"
							+     ",�t���O�P = ' ' \n"
							+     ",�t���O�Q = ' ' \n"
							+     ",�t���O�R = ' ' \n"
							+       ",�폜�e�f = '0' "
							+       ",�o�^���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�o�^�o�f = '����o�^' "
							+       ",�o�^�� = '" + sKey[8] + "' "
							+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�X�V�o�f = '����o�^' \n"
							+       ",�X�V�� = '" + sKey[8] + "'"
							+ " WHERE ����b�c = '" + sKey[0] + "' \n"
							+   " AND ����b�c = '" + sKey[1] + "' \n";

							CmdUpdate(sUser, conn2, cmdQuery);
						}
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
						sRet[0] = "����I��";
					}
					else
					{
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);

				if (sRet[0].Equals("����I��"))
				{
					logWriter(sUser, INF, "�L���̏����f�[�^�o�^�J�n");

					//�L���̏����f�[�^�̌���
					cmdQuery
						= "SELECT �L���b�c "
						+      ", �L�� "
						+   "FROM �r�l�O�R�L�� "
						+  "WHERE ����b�c = 'default' "
						+    "AND ����b�c = '0000' "
						+    "AND �폜�e�f = '0' \n";

					OracleDataReader readerDef = CmdSelect(sUser, conn2, cmdQuery);
					string s�����L���b�c = "";
					string s�����L��     = "";
					while (readerDef.Read())
					{
						s�����L���b�c = readerDef.GetString(0);
						s�����L��     = readerDef.GetString(1);

						//�L���̌���
						cmdQuery
							= "SELECT * "
							+   "FROM �r�l�O�R�L�� "
							+  "WHERE ����b�c = '" + sKey[0] + "' "
							+    "AND ����b�c = '" + sKey[1] + "' "
							+    "AND �L���b�c = '" + s�����L���b�c + "' "
						    +    "FOR UPDATE \n";

						OracleDataReader readerNote = CmdSelect(sUser, conn2, cmdQuery);
						if (readerNote.Read())
						{
							//���ɋL��������ꍇ�͐V�K�X�V
							cmdQuery
								= "UPDATE �r�l�O�R�L�� \n"
								+   " SET �L�� = '" + s�����L�� + "' "
								+       ",�폜�e�f = '0' "
								+       ",�o�^���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+       ",�o�^�o�f = '�����L��' "
								+       ",�o�^�� = '" + sKey[8] + "' "
								+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+       ",�X�V�o�f = '�����L��' "
								+       ",�X�V�� = '" + sKey[8] + "' \n"
								+ " WHERE ����b�c = '" + sKey[0] + "' \n"
								+   " AND ����b�c = '" + sKey[1] + "' \n"
								+   " AND �L���b�c = '" + s�����L���b�c + "' \n";

							CmdUpdate(sUser, conn2, cmdQuery);
							sRet[0] = "����I��";
						}
						else
						{
							//�V�K�ǉ�
							cmdQuery
								= "INSERT INTO �r�l�O�R�L�� \n"
								+ " VALUES ('" + sKey[0] + "' " 
								+         ",'" + sKey[1] + "' "
								+         ",'" + s�����L���b�c + "' "
								+         ",'" + s�����L�� + "' "
								+         ",'0' "
								+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+         ",'�����L��' "
								+         ",'" + sKey[8] + "' "
								+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+         ",'�����L��' "
								+         ",'" + sKey[8] + "' "
								+ " ) \n";

							CmdUpdate(sUser, conn2, cmdQuery);
							sRet[0] = "����I��";
						}
						logWriter(sUser, INF, sRet[0]);
					}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				}
				tran.Commit();
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�X�V
		 * �����F����b�c�A����b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�Q���� \n"
					+   " SET ���喼 = '" + sKey[2] + "' "
					+       ",�o�͏� =  " + sKey[3] 
					+       ",�X�֔ԍ� = '" + sKey[4] + "' "
					+       ",�ݒu��Z���P = '" + sKey[5] + "' "
					+       ",�ݒu��Z���Q = '" + sKey[6] + "' "
					+       ",�T�[�}���䐔 =  " + sKey[9] + "  "
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//					;
//					if(sKey.Length > 10){
//						cmdQuery = cmdQuery
//						+     ",�V���A���ԍ��P = '" + sKey[10] + "' \n"
//						+     ",��ԂP = '" + sKey[11] + "' \n"
//						+     ",�V���A���ԍ��Q = '" + sKey[12] + "' \n"
//						+     ",��ԂQ = '" + sKey[13] + "' \n"
//						+     ",�V���A���ԍ��R = '" + sKey[14] + "' \n"
//						+     ",��ԂR = '" + sKey[15] + "' \n"
//						+     ",�V���A���ԍ��S = '" + sKey[16] + "' \n"
//						+     ",��ԂS = '" + sKey[17] + "' \n"
//						+     ",�g�p�� = " + sKey[18] + "  \n"
//						;
//					}
//				cmdQuery = cmdQuery
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[8] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND ����b�c = '" + sKey[1] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� =  " + sKey[7] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
					if(sKey.Length > 10){
						cmdQuery
						= "UPDATE �b�l�O�U����g�� SET \n"
						+     " �V���A���ԍ��P = '" + sKey[10] + "' \n"
						+     ",��ԂP = '" + sKey[11] + "' \n"
						+     ",�V���A���ԍ��Q = '" + sKey[12] + "' \n"
						+     ",��ԂQ = '" + sKey[13] + "' \n"
						+     ",�V���A���ԍ��R = '" + sKey[14] + "' \n"
						+     ",��ԂR = '" + sKey[15] + "' \n"
						+     ",�V���A���ԍ��S = '" + sKey[16] + "' \n"
						+     ",��ԂS = '" + sKey[17] + "' \n"
						+     ",�g�p�� = " + sKey[18] + "  \n"
						+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+       ",�X�V�o�f = '����X�V' "
						+       ",�X�V�� = '" + sKey[8] + "' \n"
						+ " WHERE ����b�c = '" + sKey[0] + "' \n"
						+   " AND ����b�c = '" + sKey[1] + "' \n"
						+   " AND �폜�e�f = '0' \n"
						;
//						+   " AND �X�V���� =  " + sKey[7] + " \n";

						if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
						{
							;
						}
					}
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�폜
		 * �����F����b�c�A����b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�폜�J�n");
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� START
			logWriter(sUser, INF, "�Z�N�V�����폜�@["+sKey[0]+"]["+sKey[1]+"]");
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�Q���� \n"
					+    "SET �폜�e�f = '1' "
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[3] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' "
					+   " AND ����b�c = '" + sKey[1] + "' "
					+   " AND �X�V���� = " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
					cmdQuery
						= "UPDATE �b�l�O�U����g�� \n"
						+    "SET �폜�e�f = '1' "
						+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+       ",�X�V�o�f = '����X�V' "
						+       ",�X�V�� = '" + sKey[3] + "' \n"
						+ " WHERE ����b�c = '" + sKey[0] + "' "
						+   " AND ����b�c = '" + sKey[1] + "' ";

					if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
					{
						;
					}
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ���p�҃}�X�^����
		 * �����F����b�c�A���p�҂b�c
		 * �ߒl�F�X�e�[�^�X�A���p�҂b�c�A�p�X���[�h�A���p�Җ�...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���p�҃}�X�^�����J�n");

			OracleConnection conn2 = null;
// MOD 2005.07.21 ���s�j���� �X�����[�U�Ή� START
//			string[] sRet = new string[9];
// MOD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� START
//			string[] sRet = new string[10];
			string[] sRet = new string[11];
// MOD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� END
// MOD 2005.07.21 ���s�j���� �X�����[�U�Ή� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM04.���p�҂b�c "
					+       ",CM04.\"�p�X���[�h\" "
					+       ",CM04.���p�Җ� "
					+       ",CM04.����b�c "
					+       ",NVL(CM02.���喼, ' ') "
					+       ",CM04.�ב��l�b�c "
					+       ",CM04.�F�؃G���[�� "
					+       ",CM04.�X�V���� \n"
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� START
					+       ",CM04.�����P \n"
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� END
// ADD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� START
					+       ",CM04.�o�^�o�f \n"
// ADD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� END
					+  " FROM �b�l�O�S���p�� CM04 \n"
					+  " LEFT JOIN  �b�l�O�Q���� CM02 \n"
					+    " ON CM04.����b�c = CM02.����b�c "
					+    "AND CM04.����b�c = CM02.����b�c "
					+    "AND CM02.�폜�e�f = '0' \n"
					+ " WHERE CM04.����b�c = '" + sKey[0] + "' \n"
					+   " AND CM04.���p�҂b�c = '" + sKey[1] + "' \n"
					+   " AND CM04.�폜�e�f   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetDecimal(6).ToString().Trim();
					sRet[8] = reader.GetDecimal(7).ToString().Trim();
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� START
					sRet[9] = reader.GetString(8).Trim();
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� END
// ADD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� START
					sRet[10] = reader.GetString(9).Trim();
// ADD 2008.05.29 ���s�j���� �p�X���[�h�X�V�N������\�� END
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� START
		/*********************************************************************
		 * ���p�҃}�X�^�����Q
		 * �����F����b�c�A���p�҂b�c
		 * �ߒl�F�X�e�[�^�X�A���p�҂b�c�A�p�X���[�h�A���p�Җ�...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_User2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "���p�҃}�X�^�����Q�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[14];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM04.���p�҂b�c "
					+       ",CM04.\"�p�X���[�h\" "
					+       ",CM04.���p�Җ� "
					+       ",CM04.����b�c "
					+       ",NVL(CM02.���喼, ' ') "
					+       ",CM04.�ב��l�b�c "
					+       ",CM04.�F�؃G���[�� "
					+       ",CM04.�X�V���� \n"
					+       ",CM04.�����P \n"
					+       ",CM04.�o�^�o�f \n"
					+       ",NVL(CM02.�X�֔ԍ�, ' ') "
					+       ",NVL(CM14.�X���b�c, ' ') "
					+       ",NVL(CM10.�X����, ' ') "
					+  " FROM �b�l�O�S���p�� CM04 \n"
					+  " LEFT JOIN  �b�l�O�Q���� CM02 \n"
					+    " ON CM04.����b�c = CM02.����b�c "
					+    "AND CM04.����b�c = CM02.����b�c "
					+    "AND CM02.�폜�e�f = '0' \n"
					+  " LEFT JOIN �b�l�P�S�X�֔ԍ� CM14 \n"
					+    " ON CM02.�X�֔ԍ� = CM14.�X�֔ԍ� "
//					+   " AND CM14.�폜�e�f = '0' \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
					+    " ON CM14.�X���b�c = CM10.�X���b�c "
//					+   " AND CM10.�폜�e�f = '0' \n"
					+ " WHERE CM04.����b�c = '" + sKey[0] + "' \n"
					+   " AND CM04.���p�҂b�c = '" + sKey[1] + "' \n"
					+   " AND CM04.�폜�e�f   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetDecimal(6).ToString().Trim();
					sRet[8] = reader.GetDecimal(7).ToString().Trim();
					sRet[9] = reader.GetString(8).Trim();
					sRet[10] = reader.GetString(9).Trim();
					sRet[11] = reader.GetString(10).Trim();
					sRet[12] = reader.GetString(11).Trim();
					sRet[13] = reader.GetString(12).Trim();

					iCnt++;
				}
				disposeReader(reader);
				reader = null;

				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� END

		/*********************************************************************
		 * ���p�҃}�X�^�ꗗ�擾
		 * �����F����b�c�A���p�҂b�c�A���p�Җ�
		 * �ߒl�F�X�e�[�^�X�A���p�҂b�c�A���p�Җ��A����b�c�A�ב��l�b�c
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���p�҃}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(���p�҂b�c) || '|' "
					+     "|| TRIM(���p�Җ�) || '|' "
					+     "|| TRIM(����b�c) || '|' "
					+     "|| TRIM(�ב��l�b�c) || '|' \n"
					+  " FROM �b�l�O�S���p�� \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n";
				if (sKey[1].Trim().Length == 4)
				{
					cmdQuery += " AND ���p�҂b�c = '" + sKey[1] + "' \n";
				}
				else
				{
					cmdQuery += " AND ���p�҂b�c LIKE '" + sKey[1] + "%' \n";
				}
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND ���p�Җ� LIKE '" + sKey[2] + "%' \n";
				}
// ADD 2005.06.13 ���s�j�����J ����b�c�ǉ� START
				if (sKey[3].Trim().Length != 0)
				{
					cmdQuery += " AND ����b�c = '" + sKey[3] + "' \n";
				}
// ADD 2005.06.13 ���s�j�����J ����b�c�ǉ� END
				cmdQuery += " AND �폜�e�f = '0' \n";
				cmdQuery += " ORDER BY ���p�҂b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				sRet = new string[sList.Count + 1];

				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� START
		/*********************************************************************
		 * ���p�҃}�X�^�ꗗ�擾�Q
		 * �����F����b�c�A�X���b�c
		 * �ߒl�F�X�e�[�^�X�A����b�c�A���喼�A���p�҂b�c�A���p�Җ�
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_User2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "���p�҃}�X�^�ꗗ�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| ' ' || TRIM(CM04.����b�c) || '|' "
					+     "|| TRIM(CM02.���喼) || '|' "
					+     "|| ' ' || TRIM(CM04.���p�҂b�c) || '|' "
					+     "|| TRIM(CM04.���p�Җ�) || '|' "
					+  " FROM �b�l�O�S���p�� CM04 \n"
					+      ", �b�l�O�Q���� CM02 \n"
					+      ", �b�l�P�S�X�֔ԍ� CM14 \n"
					+ " WHERE CM04.����b�c = '" + sKey[0] + "' \n";
				cmdQuery += " AND CM04.�폜�e�f = '0' \n";
				cmdQuery += " AND CM04.����b�c = CM02.����b�c \n";
				cmdQuery += " AND CM04.����b�c = CM02.����b�c \n";
				cmdQuery += " AND CM02.�폜�e�f = '0' \n";
				cmdQuery += " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n";
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM14.�X���b�c = '" + sKey[1] + "' \n";
				}
//				cmdQuery += " AND CM14.�폜�e�f = '0' \n";
				cmdQuery += " ORDER BY CM04.����b�c, CM04.���p�҂b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� END
		/*********************************************************************
		 * ���p�҃}�X�^�ǉ�
		 * �����F����b�c�A���p�҂b�c�A���p�Җ�
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���p�҃}�X�^�ǉ��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f "
					+   "FROM �b�l�O�S���p�� "
					+  "WHERE ����b�c = '" + sKey[0] + "' "
					+    "AND ���p�҂b�c = '" + sKey[1] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				while (reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �b�l�O�S���p�� \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'" + sKey[3] + "' "
						+         ",'" + sKey[4] + "' "
						+         ",'" + sKey[5] + "' "
						+         ","  + sKey[6]
// MOD 2005.07.21 ���s�j���� �X�����[�U�Ή� START
//						+         ",' ' "
						+         ",'" + sKey[9] + "' \n"
// MOD 2005.07.21 ���s�j���� �X�����[�U�Ή� END
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",'0' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� START
//						+         ",'����o�^' "
						+ "\n";
					if(sKey.Length > 10){
						cmdQuery += ",'" + sKey[10] + "' \n";
					}else{
						cmdQuery += ",TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
					cmdQuery = cmdQuery
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� END
						+         ",'" + sKey[8] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'����o�^' "
						+         ",'" + sKey[8] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �b�l�O�S���p�� \n"
							+   " SET �p�X���[�h = '" + sKey[2] + "' "
							+       ",���p�Җ� = '" + sKey[3] + "' "
							+       ",����b�c = '" + sKey[4] + "' "
							+       ",�ב��l�b�c = '" + sKey[5] + "' "
							+       ",�F�؃G���[�� = " + sKey[6]
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� START
							+       ",�����P = '" + sKey[9] + "' \n"
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� END
							+       ",�폜�e�f = '0' "
							+       ",�o�^���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� START
//							+       ",�o�^�o�f = '����o�^' "
							+ "\n";
						if(sKey.Length > 10){
							cmdQuery += ",�o�^�o�f = '" + sKey[10] + "' \n";
						}else{
							cmdQuery += ",�o�^�o�f = TO_CHAR(SYSDATE,'YYYYMMDD') \n";
						}
						cmdQuery = cmdQuery
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� END
							+       ",�o�^�� = '" + sKey[8] + "' "
							+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�X�V�o�f = '����o�^' "
							+       ",�X�V�� = '" + sKey[8] + "' \n"
							+ " WHERE ����b�c = '" + sKey[0] + "' \n"
							+   " AND ���p�҂b�c = '" + sKey[1] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "����I��";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ���p�҃}�X�^�X�V
		 * �����F����b�c�A���p�҂b�c�A�p�X���[�h...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���p�҃}�X�^�X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�S���p�� \n"
					+   " SET �p�X���[�h = '" + sKey[2] + "' "
					+       ",���p�Җ� = '" + sKey[3] + "' " 
					+       ",����b�c = '" + sKey[4] + "' "
					+       ",�ב��l�b�c = '" + sKey[5] + "' "
					+       ",�F�؃G���[�� = " + sKey[6]
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� START
					+       ",�����P = '" + sKey[9] + "' \n"
// ADD 2005.07.21 ���s�j���� �X�����[�U�Ή� END
// ADD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� START
					;
				if(sKey.Length > 10){
					cmdQuery += ",�o�^�o�f = '" + sKey[10] + "' \n";
				}
				cmdQuery = cmdQuery
// ADD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� END
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[8] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND ���p�҂b�c = '" + sKey[1] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� =  " + sKey[7] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� START
		/*********************************************************************
		 * ���p�҃}�X�^�X�V
		 * �����F����b�c�A���p�҂b�c�A�p�X���[�h...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_User2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "���p�҃}�X�^�X�V�Q�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�S���p�� \n"
					+   " SET �F�؃G���[�� = " + sKey[3] + " \n"
					+       ",�o�^�o�f = '" + sKey[4] + "' \n"
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') \n"
					+       ",�X�V�o�f = '" + sKey[5] + "' \n"
					+       ",�X�V�� = '" + sKey[6] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND ���p�҂b�c = '" + sKey[1] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� =  " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2008.12.08 ���s�j���� �c�Ə��ł̃p�X���[�h�Ɖ�Ή� END

		/*********************************************************************
		 * ���p�҃}�X�^�폜
		 * �����F����b�c�A���p�҂b�c�A�X�V�����A�X�V��
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���p�҃}�X�^�폜�J�n");
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� START
			logWriter(sUser, INF, "���[�U�폜 ["+sKey[0]+"]["+sKey[1]+"]");
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�S���p�� \n"
					+   " SET �폜�e�f = '1' \n"
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[3] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND ���p�҂b�c = '" + sKey[1] + "' \n"
					+   " AND �X�V���� = " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �X���}�X�^�擾
		 * �����F�X���b�c
		 * �ߒl�F�X�e�[�^�X�A�X����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Shop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X���}�X�^�����J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �X���� "
					+   "FROM �b�l�P�O�X�� "
					+  "WHERE �X���b�c = '" + sKey[0] + "' "
					+    "AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

// ADD 2014.09.10 BEVAS)�O�c �x�X�~�ߑΉ� START
		/*********************************************************************
		 * �x�X�~�ߑΉ��󋵂̎擾
		 * �����F�X���b�c
		 * �ߒl�F�X�e�[�^�X�A
		 * �@�@�@�x�X�~�߂e�f�P("0" ���@��Ή� / "1" �� "�Ή�")
		 * �@�@�@�x�X�~�߂e�f�Q("0" ���@��Ή� / "1" �� "�Ή�")
		 *       �X�֔ԍ�
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_GeneralDelivery(string[] sUser, string[] sKey)
		{
			// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
			//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X���}�X�^����̎x�X�~�ߑΉ������J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[4];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
				//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
			// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
			//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
			//			// ����`�F�b�N
			//			sRet[0] = userCheck2(conn2, sUser);
			//			if(sRet[0].Length > 0)
			//			{
			//				disconnect2(sUser, conn2);
			//				logFileClose();
			//				return sRet;
			//			}
			//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �x�X�~�߂e�f�P,�x�X�~�߂e�f�Q,�X�֔ԍ� \n"
					+  " FROM �b�l�P�O�X�� \n"
					+  " WHERE �X���b�c = '" + sKey[0] + "' \n"
					+  " AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
	
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					iCnt++;
				}
				// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
				// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else 
				{
					sRet[0] = "����I��";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
				// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
				//				logFileClose();
			}
			return sRet;
		}
// ADD 2014.09.10 BEVAS)�O�c �x�X�~�ߑΉ� END

		/*********************************************************************
		 * �X���}�X�^�ꗗ�擾
		 * �����F�X���b�c�A�X����
		 * �ߒl�F�X�e�[�^�X�A�X���b�c�A�X����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Shop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X���}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(�X���b�c) || '|' "
// MOD 2009.11.16 ���s�j���� �W��X���ꗗ�ɒǉ� START
//					+     "|| TRIM(�X����) || '|' \n"
					+     "|| TRIM(�X����) || '|' "
					+     "|| TRIM(�W��X�b�c) || '|' "
					+     "\n"
// MOD 2009.11.16 ���s�j���� �W��X���ꗗ�ɒǉ� END
					+  " FROM �b�l�P�O�X�� \n";
				if (sKey[0].Length == 4)
				{
					cmdQuery += " WHERE �X���b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE �X���b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length != 0)
				{
					cmdQuery += " AND �X���� LIKE '" + sKey[1] + "%' \n";
				}
				cmdQuery += " AND �폜�e�f = '0' \n"
						 +  " ORDER BY �X���b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

// ADD 2014.09.10 BEVAS)�O�c �s���{�����̓X���ꗗ�擾 START
		/*********************************************************************
		 * �s���{�����̓X���}�X�^�ꗗ�擾
		 * �����F�s���{����
		 * �ߒl�F�X�e�[�^�X�A�X���b�c�A�X����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_PrefShop(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�X���}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(�X���b�c) || '|' "
					+     "|| TRIM(�X����) || '|' "
					+     "|| TRIM(�W��X�b�c) || '|' "
					+     "|| TRIM(�Z��) || '|' "
					+     "\n"
					+  " FROM �b�l�P�O�X�� \n"
					+  " WHERE �폜�e�f = '0' \n";
				if (sKey[0].Length > 0)
				{
					cmdQuery += " AND �Z�� LIKE '" + sKey[0] + "%' \n";
				}
				else
				{

				}
				cmdQuery += " ORDER BY �X���b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}


// ADD 2014.09.10 BEVAS)�O�c �s���{�����̓X���ꗗ�擾 END

		/*********************************************************************
		 * ������}�X�^�ꗗ�擾
		 * �����F�X���b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�X�֔ԍ��A���Ӑ�b�c�j...
		 *
		 * �Q�ƌ��F������}�X�^.cs ���ݖ��g�p
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Claim(string[] sUser, string sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "������}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' || TRIM(SM04.�X�֔ԍ�) || '|' "
					+     "|| TRIM(SM04.���Ӑ�b�c)     || '|' "
					+     "|| TRIM(SM04.���Ӑ敔�ۂb�c) || '|' "
					+     "|| TRIM(SM04.���Ӑ敔�ۖ�)   || '|' "
					+     "|| TRIM(SM04.����b�c) || '|' "
					+     "|| NVL(CM01.�����, ' ')  || '|' "
					+     "|| TO_CHAR(SM04.�X�V����) || '|' \n"
					+  " FROM �b�l�P�S�X�֔ԍ� CM14 "
					+      ", �r�l�O�S������ SM04 \n"
					+  " LEFT JOIN �b�l�O�P��� CM01 \n"
					+    " ON SM04.����b�c = CM01.����b�c "
					+    "AND '0' = CM01.�폜�e�f \n"
					+ " WHERE CM14.�X���b�c = '" + sKey + "' \n"
					+   " AND CM14.�X�֔ԍ� = SM04.�X�֔ԍ� \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+   " AND CM14.�폜�e�f = '0' \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					+   " AND SM04.�폜�e�f = '0' \n"
					+ " ORDER BY SM04.����b�c "
					+          ",SM04.���Ӑ�b�c "
					+          ",SM04.���Ӑ敔�ۂb�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// MOD 2007.01.18 ���s�j���� ��ʃ��C�A�E�g�ύX START
		/*********************************************************************
		 * ������}�X�^�ꗗ�擾�Q
		 * �����F�X���b�c�A����b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�X�֔ԍ��A���Ӑ�b�c�j...
		 *
		 * �Q�ƌ��F������}�X�^.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Claim2(string[] sUser, string sTensyo, string sKaiin)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "������}�X�^�ꗗ�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' || TRIM(SM04.�X�֔ԍ�) || '|' "
// MOD 2006.12.15 ������}�X�^�ꗗ�̕ύX START
//					+     "|| TRIM(SM04.���Ӑ�b�c)     || '|' "
//					+     "|| TRIM(SM04.���Ӑ敔�ۂb�c) || '|' "
//					+     "|| TRIM(SM04.���Ӑ敔�ۖ�)   || '|' "
//					+     "|| TRIM(SM04.����b�c) || '|' "
//					+     "|| NVL(CM01.�����, ' ')  || '|' "
					+     "|| TRIM(SM04.����b�c) || '|' "
// MOD 2007.01.22 ������}�X�^�ꗗ�̕ύX START
//					+     "|| NVL(CM01.�����, ' ')  || '|' "
					+     "|| NVL(TRIM(CM01.�����), ' ')  || '|' "
// MOD 2007.01.22 ������}�X�^�ꗗ�̕ύX END
					+     "|| TRIM(SM04.���Ӑ�b�c)     || '|' "
					+     "|| TRIM(SM04.���Ӑ敔�ۂb�c) || '|' "
					+     "|| TRIM(SM04.���Ӑ敔�ۖ�)   || '|' "
// MOD 2006.12.15 ������}�X�^�ꗗ�̕ύX END
					+     "|| TO_CHAR(SM04.�X�V����) || '|' \n"
					+  " FROM �b�l�P�S�X�֔ԍ� CM14 "
					+      ", �r�l�O�S������ SM04 \n"
					+  " LEFT JOIN �b�l�O�P��� CM01 \n"
					+    " ON SM04.����b�c = CM01.����b�c "
					+    "AND '0' = CM01.�폜�e�f \n"
					+ " WHERE CM14.�X���b�c = '" + sTensyo + "' \n";

				if(sKaiin.Length > 0){
					cmdQuery += "AND  SM04.����b�c = '" + sKaiin + "' \n";
				}
				cmdQuery
					+=  " AND CM14.�X�֔ԍ� = SM04.�X�֔ԍ� \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+   " AND CM14.�폜�e�f = '0' \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					+   " AND SM04.�폜�e�f = '0' \n"
// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� START
				    +   " AND CM01.�Ǘ��ҋ敪 IN ('0','1','2') \n"
// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� END
					+ " ORDER BY SM04.����b�c "
					+          ",SM04.���Ӑ�b�c "
					+          ",SM04.���Ӑ敔�ۂb�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// MOD 2007.01.18 ���s�j���� ��ʃ��C�A�E�g�ύX END

		/*********************************************************************
		 * ������}�X�^�ǉ�
		 * �����F�X�֔ԍ��A���Ӑ�b�c�A���Ӑ敔�ۂb�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Claim(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "������}�X�^�ǉ��J�n");

// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
			string s����b�c = (sKey.Length > 9) ? sKey[9] : sKey[4];
			if(s����b�c.Trim().Length == 0) s����b�c = sKey[4];
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f "
					+   "FROM �r�l�O�S������ "
					+  "WHERE �X�֔ԍ� = '" + sKey[0] + "' "
					+    "AND ���Ӑ�b�c = '" + sKey[1] + "' "
					+    "AND ���Ӑ敔�ۂb�c = '" + sKey[2] + "' "
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
					+    "AND ����b�c = '" + s����b�c + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				while (reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �r�l�O�S������ \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'" + sKey[3] + "' "
						+         ",'" + sKey[4] + "' "
						+         ",'0' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'" + sKey[5] + "' "
						+         ",'" + sKey[6] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'" + sKey[5] + "' "
						+         ",'" + sKey[6] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �r�l�O�S������ \n"
							+   " SET �X�֔ԍ� = '" + sKey[0] + "' "
							+       ",���Ӑ敔�ۖ� = '" + sKey[3] + "' " 
							+       ",����b�c = '" + sKey[4] + "' "
							+       ",�폜�e�f = '0' "
							+       ",�o�^���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�o�^�o�f = '" + sKey[5] + "' "
							+       ",�o�^�� = '" + sKey[6] + "' "
							+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",�X�V�o�f = '" + sKey[5] + "' "
							+       ",�X�V�� = '" + sKey[6] + "' \n"
							+ " WHERE �X�֔ԍ� = '" + sKey[0] + "' \n"
							+   " AND ���Ӑ�b�c = '" + sKey[1] + "' \n"
							+   " AND ���Ӑ敔�ۂb�c = '" + sKey[2] + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
							+   " AND ����b�c = '" + s����b�c + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
							;

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "����I��";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ������}�X�^�X�V
		 * �����F�X�֔ԍ��A���Ӑ�b�c�A���Ӑ敔�ۂb�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Claim(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "������}�X�^�X�V�J�n");

// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
			string s����b�c = (sKey.Length > 9) ? sKey[9] : sKey[4];
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				bool b�X�V = true;
				if (!sKey[0].Equals(sKey[7]))
				{
					//�X�֔ԍ����ύX����Ă����ꍇ
					cmdQuery
						= "SELECT �폜�e�f "
						+   "FROM �r�l�O�S������ "
						+  "WHERE �X�֔ԍ� = '" + sKey[0] + "' "
						+    "AND ���Ӑ�b�c = '" + sKey[1] + "' "
						+    "AND ���Ӑ敔�ۂb�c = '" + sKey[2] + "' "
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
						+   " AND ����b�c = '" + s����b�c + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
						+    "FOR UPDATE \n";

					OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
					int iCnt = 1;
					string s�폜�e�f = "";
					while (reader.Read())
					{
						s�폜�e�f = reader.GetString(0);
						iCnt++;
					}
					if(iCnt == 1) b�X�V = true;
					else
					{
						if (s�폜�e�f.Equals("1"))
						{
							cmdQuery
								= "DELETE FROM �r�l�O�S������ "
								+  "WHERE �X�֔ԍ� = '" + sKey[0] + "' "
								+    "AND ���Ӑ�b�c = '" + sKey[1] + "' "
								+    "AND ���Ӑ敔�ۂb�c = '" + sKey[2] + "' "
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
								+   " AND ����b�c = '" + s����b�c + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
								+    "AND �폜�e�f = '1' ";

							if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
							{
								b�X�V = true;
							}
							else
							{
								sRet[0] = "�Y���f�[�^������܂���";
								b�X�V = false;
							}
						}
						else
						{
							sRet[0] = "���ɓo�^����Ă��܂�";
							b�X�V = false;
						}
					}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				}
				if (b�X�V)
				{
					cmdQuery
						= "UPDATE �r�l�O�S������ \n"
						+   " SET �X�֔ԍ� = '" + sKey[0] + "' "
						+       ",���Ӑ敔�ۖ� = '" + sKey[3] + "' " 
						+       ",����b�c = '" + sKey[4] + "' "
						+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+       ",�X�V�o�f = '" + sKey[5] + "' "
						+       ",�X�V�� = '" + sKey[6] + "' \n"
						+ " WHERE �X�֔ԍ� = '" + sKey[7] + "' \n"
						+   " AND ���Ӑ�b�c = '" + sKey[1] + "' \n"
						+   " AND ���Ӑ敔�ۂb�c = '" + sKey[2] + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
						+   " AND ����b�c = '" + s����b�c + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
// MOD 2007.02.09 ���s�j���� ���̃`�F�b�N START
//						+   " AND �X�V���� = '" + sKey[8] + "' \n"
						+   " AND �X�V���� = " + sKey[8] + " \n"
// MOD 2007.02.09 ���s�j���� ���̃`�F�b�N END
						+   " AND �폜�e�f = '0' \n";

					if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
					{
						tran.Commit();
						sRet[0] = "����I��";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���̒[���ōX�V����Ă��܂�";
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ������}�X�^�폜
		 * �����F�X�֔ԍ��A���Ӑ�b�c�A���Ӑ敔�ۂb�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Claim(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "������}�X�^�폜�J�n");
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
			string s����b�c = (sKey.Length > 6) ? sKey[6] : "";
			if(s����b�c.Length == 0){
				return new string[]{"�A�v���̃o�[�W�������Â��ׁA�폜�ł��܂���B"};
			}
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� START
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
//			logWriter(sUser, INF, "������폜 ["+sKey[0]+"]["+sKey[1]+"]["+sKey[2]+"]");
			logWriter(sUser, INF, "������폜 ["+sKey[0]+"]["+sKey[1]+"]["+sKey[2]+"]["+s����b�c+"]");
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
// MOD 2010.08.26 ���s�j���� ���q�l�A�Z�N�V�����A������폜���̃��O���� END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END
// MOD 2008.11.26 ���s�j���� ���ۃR�[�h���󔒂ł��G���[���łȂ����� START
			if(sKey[2].Length == 0) sKey[2] = " ";
// MOD 2008.11.26 ���s�j���� ���ۃR�[�h���󔒂ł��G���[���łȂ����� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �r�l�O�S������ \n"
					+   " SET �폜�e�f = '1' " 
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '" + sKey[3] + "' "
					+       ",�X�V�� = '" + sKey[4] + "' \n"
					+ " WHERE �X�֔ԍ� = '" + sKey[0] + "' \n"
					+   " AND ���Ӑ�b�c = '" + sKey[1] + "' \n"
					+   " AND ���Ӑ敔�ۂb�c = '" + sKey[2] + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� START
					+   " AND ����b�c = '" + s����b�c + "' \n"
// MOD 2011.03.09 ���s�j���� ������}�X�^�̎�L�[��[����b�c]��ǉ� END
					+   " AND �X�V���� = " + sKey[5] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �ב��l�}�X�^�擾
		 * �����F����b�c�A����b�c�A�ב��l�b�c
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Consignor(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�ב��l�}�X�^�����J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �ב��l�b�c "
					+   "FROM �r�l�O�P�ב��l "
					+  "WHERE ����b�c = '" + sKey[0] + "' "
// DEL 2008.12.03 ���s�j���� �ב��l���݃`�F�b�N���畔��b�c�̂��΂���͂��� START
//					+    "AND ����b�c = '" + sKey[1] + "' "
// DEL 2008.12.03 ���s�j���� �ב��l���݃`�F�b�N���畔��b�c�̂��΂���͂��� END
					+    "AND �ב��l�b�c = '" + sKey[2] + "' "
					+    "AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �ב��l�}�X�^�ꗗ�擾
		 * �����F����b�c�A����b�c�A�ב��l�b�c�A�J�i
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i���O�P�A�Z���P�A�ב��l�b�c�j...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Consignor(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�ב��l�}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(���O�P) || '|' "
					+     "|| TRIM(�Z���P) || '|' "
					+     "|| TRIM(�ב��l�b�c) || '|' "
					+     "|| '(' || TRIM(�d�b�ԍ��P) || ')' "
					+     "|| TRIM(�d�b�ԍ��Q) || '-' "
					+     "|| TRIM(�d�b�ԍ��R) || '|' "
					+     "|| TRIM(�J�i����) || '|' \n"
					+  " FROM �r�l�O�P�ב��l \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND ����b�c = '" + sKey[1] + "' \n";
				if (sKey[2].Length == 12)
				{
					cmdQuery += " AND �ב��l�b�c = '" + sKey[2] + "' \n";
				}
				else
				{
					cmdQuery += " AND �ב��l�b�c LIKE '" + sKey[2] + "%' \n";
				}
				if (sKey[3].Length != 0)
				{
// MOD 2006.06.28 ���s�j���� ���Ԉ�v�ɏC�� START
//					cmdQuery += " AND �J�i���� LIKE '" + sKey[3] + "%' \n";
					cmdQuery += " AND �J�i���� LIKE '%" + sKey[3] + "%' \n";
// MOD 2006.06.28 ���s�j���� ���Ԉ�v�ɏC�� END
				}
				cmdQuery += " AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �X�֔ԍ��}�X�^�擾
		 * �����F�X�֔ԍ�
		 * �ߒl�F�X�e�[�^�X�A�X����
		 *
		 * �Q�ƌ��F����}�X�^.cs
		 * �Q�ƌ��F������}�X�^.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Postcode(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X�֔ԍ��}�X�^�����J�n");

			OracleConnection conn2 = null;
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
//			string[] sRet = new string[3];
			string[] sRet = new string[4]{"","","",""};
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT NVL(CM10.�X����, ' '), \n"
					+ " TRIM(CM14.�s���{����) || TRIM(CM14.�s�撬����) || TRIM(CM14.���於) \n"
// ADD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
					+ ", CM14.�X���b�c \n"
// ADD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END
					+  " FROM �b�l�P�S�X�֔ԍ� CM14 \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
					+    " ON CM14.�X���b�c = CM10.�X���b�c "
					+    "AND CM10.�폜�e�f = '0' \n"
					+ " WHERE CM14.�X�֔ԍ� = '" + sKey[0] + "' \n"
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+   " AND CM14.�폜�e�f = '0' \n";
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					;

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
// ADD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
					sRet[3] = reader.GetString(2).Trim();
// ADD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �Z���}�X�^�ꗗ�擾(��)
		 * �����F�s���{���b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�s�撬�����A�s�撬���b�c�j...
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Get_byKen(string[] sUser, string s�s���{���b�c)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�Z���}�X�^�ꗗ�擾(��)�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '||' || TRIM(�s�撬����) || '|' "
					+             "|| TRIM(�s�撬���b�c) || '|' "
					+             "|| '||' \n"
					+  " FROM �b�l�P�Q�s�撬�� \n"
					+ " WHERE �s���{���b�c = '" + s�s���{���b�c + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+ " ORDER BY �s�撬���b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0).Trim());
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �Z���}�X�^�ꗗ�擾(�s)
		 * �����F�s���{���b�c�A�s�撬���b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�X�֔ԍ��A�厚�ʏ̖��j...
		 *
		 *********************************************************************/
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX START
		private static string GET_BYKENSHI_SELECT
			= "SELECT '|' || TRIM(MAX(CM13.�X�֔ԍ�)) || '|' "
			+ "|| TRIM(MAX(CM13.�厚�ʏ̖�)) || '|' "
			+ "|| TRIM(MAX(CM13.�s���{���b�c))"
			+ "|| TRIM(MAX(CM13.�s�撬���b�c))"
			+ "|| TRIM(MAX(CM13.�厚�ʏ̂b�c)) || '|' "
			+ "|| MIN(NVL(CM10.�X����, ' ')) || '|' \n"
			+  " FROM �b�l�P�R�Z�� CM13 \n"
			+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
			+    " ON CM13.�X���b�c = CM10.�X���b�c "
			+    "AND CM10.�폜�e�f = '0' \n"
			;
		private static string GET_BYKENSHI_WHERE
			= " AND CM13.�폜�e�f = '0' \n"
			+ " GROUP BY CM13.�厚�ʏ̂b�c \n"
			+ " ORDER BY CM13.�厚�ʏ̂b�c \n"
			;
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX END
		[WebMethod]
		public String[] Get_byKenShi(string[] sUser, string s�s���{���b�c, string s�s�撬���b�c)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�Z���}�X�^�ꗗ�擾(�s)�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX START
//					= "SELECT '|' || TRIM(CM13.�X�֔ԍ�) || '|' "
//					+ "|| TRIM(CM13.�厚�ʏ̖�) || '|' "
//					+ "|| TRIM(CM13.�s���{���b�c) || TRIM(CM13.�s�撬���b�c) || TRIM(CM13.�厚�ʏ̂b�c) || '|' "
//					+ "|| NVL(CM10.�X����, ' ') || '|' \n"
//					+  " FROM �b�l�P�R�Z�� CM13 \n"
//					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
//					+    " ON CM13.�X���b�c = CM10.�X���b�c "
//					+    "AND CM10.�폜�e�f = '0' \n"
					= GET_BYKENSHI_SELECT
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX END
					+ " WHERE CM13.�s���{���b�c = '" + s�s���{���b�c + "' \n"
					+   " AND CM13.�s�撬���b�c = '" + s�s�撬���b�c + "' \n"
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX START
//					+   " AND CM13.�폜�e�f = '0' \n"
//					+ " ORDER BY CM13.�厚�ʏ̂b�c \n";
					+ GET_BYKENSHI_WHERE
					;
// MOD 2009.06.23 ���s�j���� �Z���}�X�^�̃v���C�}���[�L�[�ύX END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0).Trim());
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �Z���}�X�^�ꗗ�擾
		 * �����F�X�֔ԍ�
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�X�֔ԍ��A�s���{�����j...
		 *
		 * �Q�ƌ��F�Z������.cs
		 *********************************************************************/
		[WebMethod]
		public String[] Get_byPostcode(string[] sUser, string s�X�֔ԍ�)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�Z���}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' || TRIM(CM13.�X�֔ԍ�) || '|' "
					+ "|| TRIM(CM13.�s���{����) || TRIM(CM13.�s�撬����) || TRIM(CM13.�厚�ʏ̖�) || '|' "			//�Z��
					+ "|| TRIM(CM13.�s���{���b�c) || TRIM(CM13.�s�撬���b�c) || TRIM(CM13.�厚�ʏ̂b�c) || '|' "	//�Z���b�c
					+ "|| NVL(CM10.�X����, ' ') || '|' \n"
					+  " FROM �b�l�P�R�Z�� CM13 \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
					+    " ON CM13.�X���b�c = CM10.�X���b�c "
					+    "AND CM10.�폜�e�f = '0' \n";
				if(s�X�֔ԍ�.Length == 7)
				{
					cmdQuery += " WHERE CM13.�X�֔ԍ� = '" + s�X�֔ԍ� + "' \n";
				}
				else
				{
					cmdQuery +=  " WHERE CM13.�X�֔ԍ� LIKE '" + s�X�֔ԍ� + "%' \n";
				}
				cmdQuery +=    " AND CM13.�폜�e�f = '0' \n"
					+  " ORDER BY �厚�ʏ̂b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0).Trim());
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
		/*********************************************************************
		 * �W��X�}�X�^�擾
		 * �����F�W�דX�b�c
		 * �ߒl�F�X�e�[�^�X�A�W�דX�b�c�A�X����...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�W��X�}�X�^�����J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[7];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM11.�W�דX�b�c "
					+      ", NVL(CM10S.�X����, ' ') "
					+      ", CM11.�W��X�b�c "
					+      ", NVL(CM10C.�X����, ' ') "
					+      ", CM11.�g�p�J�n�� "
					+      ", CM11.�X�V���� \n"
					+  " FROM �b�l�P�P�W��X CM11 \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10S \n"
					+    " ON CM11.�W�דX�b�c = CM10S.�X���b�c "
					+    "AND '0' = CM10S.�폜�e�f \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10C \n"
					+    " ON CM11.�W��X�b�c = CM10C.�X���b�c "
					+    "AND '0' = CM10C.�폜�e�f \n"
					+ " WHERE CM11.�W�דX�b�c = '" + sKey[0] + "' \n"
					+   " AND CM11.�폜�e�f   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetDecimal(5).ToString().Trim();
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �W��X�}�X�^�ꗗ�擾
		 * �����F�W�דX�b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�W�דX�b�c�A�X�����j...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�W��X�}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(CM11.�W�דX�b�c) || '|' "
					+     "|| NVL(CM10S.�X����, ' ') || '|' "
					+     "|| '��' || '|' "
					+     "|| TRIM(CM11.�W��X�b�c) || '|' "
					+     "|| NVL(CM10C.�X����, ' ') || '|' "
					+     "|| TRIM(CM11.�g�p�J�n��) || '|' \n"
					+  " FROM �b�l�P�P�W��X CM11 \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10S \n"
					+    " ON CM11.�W�דX�b�c = CM10S.�X���b�c "
					+    "AND '0' = CM10S.�폜�e�f \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10C \n"
					+    " ON CM11.�W��X�b�c = CM10C.�X���b�c "
					+    "AND '0' = CM10C.�폜�e�f \n"
					+ " WHERE CM11.�W�דX�b�c >= '" + sKey[0] + "' \n"
					+   " AND CM11.�폜�e�f = '0' \n"
					+ " ORDER BY CM11.�W�דX�b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �W��X�}�X�^�ǉ�
		 * �����F�W�דX�b�c�A�W��X�b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		//�W��X�}�X�^�ǉ�
		public string[] Ins_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�W��X�}�X�^�ǉ��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f "
					+   "FROM �b�l�P�P�W��X "
					+  "WHERE �W�דX�b�c = '" + sKey[0] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				while (reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �b�l�P�P�W��X \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'0' "
						+         "," + s�X�V����
						+         ",'�W��o�^' "
						+         ",'" + sKey[4] + "' "
						+         "," + s�X�V����
						+         ",'�W��o�^' "
						+         ",'" + sKey[4] + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �b�l�P�P�W��X \n"
							+   " SET �W��X�b�c = '" + sKey[1] + "' "
							+       ",�g�p�J�n�� = '" + sKey[2] + "' "
							+       ",�폜�e�f = '0'"
							+       ",�o�^���� = " + s�X�V����
							+       ",�o�^�o�f = '����o�^'"
							+       ",�o�^�� = '" + sKey[4] + "' "
							+       ",�X�V���� = " + s�X�V����
							+       ",�X�V�o�f = '����o�^' "
							+       ",�X�V�� = '" + sKey[4] + "' \n"
							+ " WHERE �W�דX�b�c = '" + sKey[0] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "����I��";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �W��X�}�X�^�X�V
		 * �����F�W�דX�b�c�A�W��X�b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�W��X�}�X�^�X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�P�P�W��X \n"
					+   " SET �W��X�b�c = '" + sKey[1] + "' "
					+       ",�g�p�J�n�� = '" + sKey[2] + "' " 
					+       ",�X�V���� =  " + s�X�V����
					+       ",�X�V�o�f = '����X�V' "
					+       ",�X�V�� = '" + sKey[4] + "' \n"
					+ " WHERE �W�דX�b�c = '" + sKey[0] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� = " + sKey[3] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �W��X�}�X�^�폜
		 * �����F�W�דX�b�c�A�X�V�����A�X�V��
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�W��X�}�X�^�폜�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�P�P�W��X "
					+    "SET �폜�e�f = '1' " 
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '�W��폜' "
					+       ",�X�V�� = '" + sKey[2] + "' "
					+  "WHERE �W�דX�b�c = '" + sKey[0] + "' "
// MOD 2007.02.09 ���s�j���� ���̃`�F�b�N START
//					+    "AND �X�V���� = '" + sKey[1] + "' \n";
					+    "AND �X�V���� = " + sKey[1] + " \n";
// MOD 2007.02.09 ���s�j���� ���̃`�F�b�N END

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ���O�C���F��
		 * �����F����b�c�A���p�҂b�c�A�p�X���[�h
		 * �ߒl�F�X�e�[�^�X�A����b�c�A������A���p�҂b�c�A���p�Җ�
		 *
		 *********************************************************************/
// ADD 2005.05.11 ���s�j���� ORA-03113�΍�H START
		private static string SET_LOGIN_SELECT1
// MOD 2006.12.07 ���s�j�����J �X���擾�폜 START
//			= "SELECT CM01.����b�c, \n"
//			+ " CM01.�����, \n"
//			+ " CM04.���p�҂b�c, \n"
//			+ " CM04.���p�Җ�, \n"
//			+ " CM14.�X���b�c  \n"
//			+ " FROM �b�l�O�P��� CM01, \n"
//			+ " �b�l�O�S���p�� CM04, \n"
//			+ " �b�l�O�Q���� CM02,  \n"
//			+ " �b�l�P�S�X�֔ԍ� CM14   \n";

			= "SELECT CM01.����b�c, \n"
			+ " CM01.�����, \n"
			+ " CM04.���p�҂b�c, \n"
			+ " CM04.���p�Җ� \n"
// ADD 2007.02.06 ���s�j���� �c�Ə�����Ή� START
			+ ", CM01.�Ǘ��ҋ敪 \n"
// ADD 2007.02.06 ���s�j���� �c�Ə�����Ή� END
// ADD 2008.03.21 ���s�j�O���[�o���Ή� START
			+ ", NVL(CM14.�X���b�c,' ') \n"
// ADD 2008.03.21 ���s�j�O���[�o���Ή� END
			+ " FROM �b�l�O�P��� CM01, \n"
// ADD 2008.03.21 ���s�j�O���[�o���Ή� START
			+ " �b�l�O�Q���� CM02, \n"
			+ " �b�l�P�S�X�֔ԍ� CM14, \n"
// ADD 2008.03.21 ���s�j�O���[�o���Ή� END
			+ " �b�l�O�S���p�� CM04 \n";
// MOD 2006.12.07 ���s�j�����J �X���擾�폜 END
// ADD 2005.05.11 ���s�j���� ORA-03113�΍�H END

		[WebMethod]
		public string[] Set_login(string[] sUser, string[] sKey) 
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���O�C���F�؊J�n");

			OracleConnection conn2 = null;
// MOD 2007.02.06 ���s�j���� �c�Ə�����Ή� START
//			string[] sRet = new string[5];
// MOD 2008.03.21 ���s�j�O���[�o���Ή� START
//			string[] sRet = new string[6];
			string[] sRet = new string[7];
// MOD 2008.03.21 ���s�j�O���[�o���Ή� END
// MOD 2007.02.06 ���s�j���� �c�Ə�����Ή� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
// MOD 2005.05.11 ���s�j���� ORA-03113�΍�H START
//				cmdQuery
//					= "SELECT CM01.����b�c "
//					+       ",CM01.����� "
//					+       ",CM04.���p�҂b�c "
//					+       ",CM04.���p�Җ� \n"
//					+  " FROM �b�l�O�P��� CM01 "
//					+       ",�b�l�O�S���p�� CM04 \n"
				cmdQuery
					= SET_LOGIN_SELECT1
// MOD 2005.05.11 ���s�j���� ORA-03113�΍�H END
					+ " WHERE CM01.����b�c = '" + sKey[0] + "' \n"
					+   " AND CM01.����b�c = CM04.����b�c \n"
					+   " AND CM04.���p�҂b�c = '" + sKey[1] + "' \n"
					+   " AND CM04.�p�X���[�h = '" + sKey[2] + "' \n"
					+   " AND CM01.�g�p�J�n�� <= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
					+   " AND CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
// MOD 2005.05.11 ���s�j���� ORA-03113�΍�H START
//					+   " AND (CM01.�Ǘ��ҋ敪 = '1' or CM01.�Ǘ��ҋ敪 = '9') \n"
// MOD 2007.02.06 ���s�j���� �c�Ə�����Ή� START
//					+   " AND CM01.�Ǘ��ҋ敪 IN ('1','9') \n"
					+   " AND CM01.�Ǘ��ҋ敪 IN ('1','2') \n"
// MOD 2007.02.06 ���s�j���� �c�Ə�����Ή� END
// MOD 2005.05.11 ���s�j���� ORA-03113�΍�H END
					+   " AND CM01.�폜�e�f = '0' \n"
					+   " AND CM04.�폜�e�f = '0' \n"
// ADD 2008.03.21 ���s�j�O���[�o���Ή� START
					+   " AND CM04.����b�c = CM02.����b�c \n"
					+   " AND CM04.����b�c = CM02.����b�c \n"
					+   " AND           '0' = CM02.�폜�e�f \n"
					+   " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ�(+) \n"
// ADD 2008.03.21 ���s�j�O���[�o���Ή� END
					;
// DEL 2006.12.07 ���s�j�����J �X���擾�폜 START
//					+   " AND CM02.����b�c = CM04.����b�c \n"
//					+   " AND CM02.����b�c = CM04.����b�c \n"
//					+   " AND CM14.�X�֔ԍ� = CM02.�X�֔ԍ� \n"
//					+   " AND CM02.�폜�e�f = '0' \n"
//					+   " AND CM14.�폜�e�f = '0' \n";
// DEL 2006.12.07 ���s�j�����J �X���擾�폜 END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
// MOD 2005.05.11 ���s�j���� ORA-03113�΍�H START
//				int iCnt = 1;
//				while (reader.Read())
//				{
//					sRet[1] = reader.GetString(0).Trim();
//					sRet[2] = reader.GetString(1).Trim();
//					sRet[3] = reader.GetString(2).Trim();
//					sRet[4] = reader.GetString(3).Trim();
//					iCnt++;
//				}
//				if(iCnt == 1) 
//				{
//					sRet[0] = "�Y���f�[�^������܂���";
//				}
//				else
//				{
//					sRet[0] = "����I��";
//				}
				if (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
// MOD 2006.12.07 ���s�j�����J �X���擾�폜 START
//					sRet[5] = reader.GetString(4).Trim();
// MOD 2006.12.07 ���s�j�����J �X���擾�폜 END
// ADD 2007.02.06 ���s�j���� �c�Ə�����Ή� START
					sRet[5] = reader.GetString(4).Trim();
// ADD 2007.02.06 ���s�j���� �c�Ə�����Ή� END
// ADD 2008.03.21 ���s�j�O���[�o���Ή� START
					sRet[6] = reader.GetString(5).Trim();
// ADD 2008.03.21 ���s�j�O���[�o���Ή� END
					sRet[0] = "����I��";
				}
				else
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
// MOD 2005.05.11 ���s�j���� ORA-03113�΍�H END
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �L���f�[�^�擾
		 * �����F����b�c�A����b�c�A�L���b�c
		 * �ߒl�F�X�e�[�^�X�A�L���b�c�A�X�V�����A��ԋ敪
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Sel_Kiji(string[] sUser, string sKCode,string sBCode,string sCode)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�L���f�[�^�擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[4];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			try
			{
				string cmdQuery
					= "SELECT �L��, TO_CHAR(�X�V����) \n"
					+  " FROM �r�l�O�R�L�� \n"
					+ " WHERE ����b�c = '" + sKCode + "' \n"
					+   " AND ����b�c = '" + sBCode + "' \n"
					+   " AND �L���b�c = '" + sCode  + "' \n"
					+   " AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				bool bRead = reader.Read();
				if(bRead == true)
				{
					sRet[1] = reader.GetString(0).TrimEnd();
					sRet[2] = reader.GetString(1);
					sRet[0] = "�X�V";
					sRet[3] = "U";
				}
				else
				{
					sRet[0] = "�o�^";
					sRet[3] = "I";
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * �L���ꗗ�擾
		 * �����F����b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�L���b�c�A�L���A�L����ʁA�X�V�����j...
		 *
		 *********************************************************************/
		private static string GET_KIJI_SELECT
			= "SELECT �L���b�c, �L��, �X�V���� \n"
			+  " FROM �r�l�O�R�L�� \n";

		private static string GET_KIJI_ORDER
			=   " AND �폜�e�f = '0' \n"
			+ " ORDER BY �L���b�c \n";

		[WebMethod]
		public String[] Get_Kiji(string[] sUser, string sKCode, string sBCode)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�L���ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			try
			{
				StringBuilder sbQuery = new StringBuilder(512);
				sbQuery.Append(GET_KIJI_SELECT);
				sbQuery.Append(" WHERE ����b�c = '" + sKCode + "' \n");
				sbQuery.Append(" AND ����b�c = '" + sBCode + "' \n");
				sbQuery.Append(GET_KIJI_ORDER);

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				StringBuilder sbData = new StringBuilder(62); // 4+30+12+4 = 50
				while (reader.Read())
				{
					sbData = new StringBuilder(62);
					sbData.Append("|" + reader.GetString(0).Trim());
					sbData.Append("|" + reader.GetString(1).TrimEnd());
					sbData.Append("|" + reader.GetDecimal(2).ToString().Trim());
					sbData.Append("|");
					sList.Add(sbData);
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}


				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * �L���f�[�^�o�^
		 * �����F����b�c�A����b�c�A�L���b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Ins_Kiji(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�L���f�[�^�o�^�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				string cmdQuery 
					= "DELETE FROM �r�l�O�R�L�� \n"
					+ " WHERE ����b�c = '" + sData[0] +"' \n"
					+   " AND ����b�c = '" + sData[1] +"' \n"
					+   " AND �L���b�c = '" + sData[2] +"' \n"
					+   " AND �폜�e�f = '1' \n";

				CmdUpdate(sUser, conn2, cmdQuery);

				cmdQuery 
					= "INSERT INTO �r�l�O�R�L��  \n"
					+ "VALUES ('" + sData[0] +"','" + sData[1] +"','" + sData[2] +"','" + sData[3] +"', \n"
					+         "'0',TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),'" + sData[4] +"','" + sData[5] +"', \n"
					+         "TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),'" + sData[4] +"','" + sData[5] +"') \n";

				CmdUpdate(sUser, conn2, cmdQuery);

				tran.Commit();
				sRet[0] = "����I��";
				
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * �L���f�[�^�X�V
		 * �����F����b�c�A����b�c�A�L���b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_Kiji(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�L���f�[�^�X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				string cmdQuery 
					= "UPDATE �r�l�O�R�L�� \n"
					+   " SET �L��     = '" + sData[3] +"', \n"
					+       " �폜�e�f = '0', \n"
					+       " �X�V�o�f = '" + sData[4] +"', \n"
					+       " �X�V��   = '" + sData[5] +"', \n"
					+       " �X�V���� =  TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')  \n"
					+ " WHERE ����b�c = '" + sData[0] +"' \n"
					+   " AND ����b�c = '" + sData[1] +"' \n"
					+   " AND �L���b�c = '" + sData[2] +"' \n"
					+   " AND �X�V���� =  " + sData[6] +" \n";

				int iUpdRow = CmdUpdate(sUser, conn2, cmdQuery);

				tran.Commit();
				if(iUpdRow == 0)
					sRet[0] = "�f�[�^�ҏW���ɑ��̒[�����X�V����Ă��܂��B\r\n�ēx�A�ŐV�f�[�^���Ăяo���čX�V���Ă��������B";
				else				
					sRet[0] = "����I��";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * �L���f�[�^�폜
		 * �����F����b�c�A����b�c�A�L���b�c�A�X�V�o�f�A�X�V��
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Del_Kiji(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�L���f�[�^�폜�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				StringBuilder sbQuery = new StringBuilder(1024);
				sbQuery.Append
					( "UPDATE �r�l�O�R�L�� \n"
					+   " SET �폜�e�f = '1', \n"
					+       " �X�V�o�f = '" + sData[3] +"', \n"
					+       " �X�V��   = '" + sData[4] +"', \n"
					+       " �X�V���� =  TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') \n"
					+ " WHERE ����b�c = '" + sData[0] +"' \n"
					+   " AND ����b�c = '" + sData[1] +"' \n"
					+   " AND �L���b�c = '" + sData[2] +"' \n");

				CmdUpdate(sUser, conn2, sbQuery);

				tran.Commit();				
				sRet[0] = "����I��";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * ������擾�i�b�r�u�o�͗p�j
		 * �����F����b�c�A�g�p�J�n���i�J�n�A�I���j�A�g�p�I�����i�J�n�A�I���j�A
		 *		 ���p�ғo�^���i�J�n�A�I���j
		 * �ߒl�F�X�e�[�^�X�A����b�c�A������A�g�p�J�n��...
		 *
		 * �Q�ƌ��F������b�r�u�o��.cs
		 *********************************************************************/
		private static string GET_KAIINCSV_SELECT
			= "SELECT R.����b�c,NVL(K.�����,' '),NVL(K.�g�p�J�n��,' '),NVL(K.�g�p�I����,' '), \n"
			+       " R.����b�c,NVL(B.���喼,' '),NVL(Y.�X���b�c,' '),NVL(T.�X����,' '), \n"
			+       " NVL(B.�ݒu��Z���P,' '),NVL(B.�ݒu��Z���Q,' '), \n"
			+       " R.���p�҂b�c,R.\"�p�X���[�h\",R.���p�Җ�,SUBSTR(R.�o�^����,1,8) \n"
// ADD 2006.12.11 ���s�j�����J �T�[�}���䐔�ǉ� START
//			+       " ,NVL(B.�T�[�}���䐔,'0')\n"
			+       " ,NVL(B.\"�T�[�}���䐔\",'0')\n"
// ADD 2006.12.11 ���s�j�����J �T�[�}���䐔�ǉ� END
// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//			+      ", NVL(B.�V���A���ԍ��P,' '), NVL(B.��ԂP,' ') \n"
//			+      ", NVL(B.�V���A���ԍ��Q,' '), NVL(B.��ԂQ,' ') \n"
//			+      ", NVL(B.�V���A���ԍ��R,' '), NVL(B.��ԂR,' ') \n"
//			+      ", NVL(B.�V���A���ԍ��S,' '), NVL(B.��ԂS,' ') \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//			+      ", NVL(B.�V���A���ԍ��P,' '), DECODE(B.��ԂP,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
//			+      ", NVL(B.�V���A���ԍ��Q,' '), DECODE(B.��ԂQ,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
//			+      ", NVL(B.�V���A���ԍ��R,' '), DECODE(B.��ԂR,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
//			+      ", NVL(B.�V���A���ԍ��S,' '), DECODE(B.��ԂS,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
			+      ", NVL(CM06.�V���A���ԍ��P,' '), DECODE(CM06.��ԂP,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
			+      ", NVL(CM06.�V���A���ԍ��Q,' '), DECODE(CM06.��ԂQ,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
			+      ", NVL(CM06.�V���A���ԍ��R,' '), DECODE(CM06.��ԂR,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
			+      ", NVL(CM06.�V���A���ԍ��S,' '), DECODE(CM06.��ԂS,'1 ','�ԕi','2 ','�s�Ǖi','3 ','�s��','4 ','���̑�','5 ','������',' ') \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� START
			+      ", DECODE(K.�Ǘ��ҋ敪,'0','���','1','�Ǘ���','2','�c�Ə�', K.�Ǘ��ҋ敪) \n"
			+      ", DECODE(K.�L���A�g�e�f,'0',' ','1','�^����\��', K.�L���A�g�e�f) \n"
			+      ", K.�o�^����, K.�X�V���� \n"
			+      ", B.�g�D�b�c, B.�X�֔ԍ�, NVL(CM06.�g�p��,0) \n"
			+      ", DECODE(CM06.����\���Ǘ��ԍ�,NULL,' ',0,' ',TO_CHAR(CM06.����\���Ǘ��ԍ�)) \n"
			+      ", B.�o�^����, B.�X�V���� \n"
			+      ", R.�ב��l�b�c \n"
			+      ", DECODE(R.�����P,' ',' ','1','���x������֎~', R.�����P) \n"
			+      ", R.\"�F�؃G���[��\" \n"
			+      ", R.�o�^�o�f \n"
			+      ", R.�o�^����, R.�X�V���� \n"
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� END
// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
			+ " FROM �b�l�O�P��� K,�b�l�O�Q���� B,�b�l�O�S���p�� R,�b�l�P�O�X�� T,�b�l�P�S�X�֔ԍ� Y \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
			+ " ,�b�l�O�U����g�� CM06 \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j EMD
			;

		[WebMethod]
		public String[] Get_csvwrite(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "������b�r�u�o�͗p�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� START
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 ���s�j�ɉ� ����`�F�b�N�ǉ� END

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			try
			{
// MOD 2005.07.13 ���s�j���� �����̕ύX START
//				sbQuery.Append(" WHERE R.����b�c = K.����b�c(+) \n");
//				sbQuery.Append(" AND R.����b�c = B.����b�c(+) \n");
//				sbQuery.Append(" AND R.����b�c = B.����b�c(+) \n");
				sbQuery.Append(" WHERE R.����b�c = K.����b�c \n");
				sbQuery.Append(" AND R.����b�c = B.����b�c \n");
				sbQuery.Append(" AND R.����b�c = B.����b�c \n");
// MOD 2005.07.13 ���s�j���� �����̕ύX END
				sbQuery.Append(" AND B.�X�֔ԍ� = Y.�X�֔ԍ�(+) \n");
				sbQuery.Append(" AND Y.�X���b�c = T.�X���b�c(+) \n");
				sbQuery.Append(" AND R.�폜�e�f = '0' \n");
// MOD 2005.07.13 ���s�j���� �����̕ύX START
//				sbQuery.Append(" AND '0' = B.�폜�e�f(+) \n");
				sbQuery.Append(" AND '0' = K.�폜�e�f \n");
				sbQuery.Append(" AND '0' = B.�폜�e�f \n");
// MOD 2005.07.13 ���s�j���� �����̕ύX END
				sbQuery.Append(" AND '0' = T.�폜�e�f(+) \n");
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//				sbQuery.Append(" AND '0' = Y.�폜�e�f(+) \n");
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
				sbQuery.Append(" AND R.����b�c = CM06.����b�c(+) \n");
				sbQuery.Append(" AND R.����b�c = CM06.����b�c(+) \n");
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
// MOD 2011.05.31 ���s�j���� ���q�^���̑Ή� START
				sbQuery.Append(" AND K.�Ǘ��ҋ敪 IN ('0','1','2') \n"); // 0:��� 1:�Ǘ��� 2:�c�Ə�
// MOD 2011.05.31 ���s�j���� ���q�^���̑Ή� END

				if(sData[0].Length > 0 && sData[1].Length > 0)
					sbQuery.Append(" AND R.����b�c  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
				else
				{
					if(sData[0].Length > 0 && sData[1].Length == 0)
						sbQuery.Append(" AND R.����b�c = '"+ sData[0] + "' \n");
				}

				if(sData[2].Length > 0 && sData[3].Length > 0)
					sbQuery.Append(" AND K.�g�p�J�n��  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
				else
				{
					if(sData[2].Length > 0 && sData[3].Length == 0)
						sbQuery.Append(" AND K.�g�p�J�n�� = '"+ sData[2] + "' \n");
				}

				if(sData[4].Length > 0 && sData[5].Length > 0)
					sbQuery.Append(" AND K.�g�p�I����  BETWEEN '"+ sData[4] + "' AND '"+ sData[5] +"' \n");
				else
				{
					if(sData[4].Length > 0 && sData[5].Length == 0)
						sbQuery.Append(" AND K.�g�p�I���� = '"+ sData[4] + "' \n");
				}

				if(sData[6].Length > 0 && sData[7].Length > 0)
					sbQuery.Append(" AND SUBSTR(R.�o�^����,1,8)  BETWEEN '"+ sData[6] + "' AND '"+ sData[7] +"' \n");
				else
				{
					if(sData[6].Length > 0 && sData[7].Length == 0)
						sbQuery.Append(" AND SUBSTR(R.�o�^����,1,8) = '"+ sData[6] + "' \n");
				}
				sbQuery.Append(" ORDER BY R.����b�c,R.���p�҂b�c ");


				OracleDataReader reader;

				sbQuery2.Append(GET_KAIINCSV_SELECT);
				sbQuery2.Append(sbQuery);
				reader = CmdSelect(sUser, conn2, sbQuery2);

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read())
				{
					sbData = new StringBuilder(1024);
					sbData.Append(sDbl + sSng + reader.GetString(0).Trim() + sDbl);				// ����b�c
					sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl);			// �����
					sbData.Append(sKanma + sDbl + reader.GetString(2).Trim() + sDbl);			// �g�p�J�n��
					sbData.Append(sKanma + sDbl + reader.GetString(3).Trim() + sDbl);			// �g�p�I����
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� START
					sbData.Append(sKanma + sDbl + reader.GetString(23).TrimEnd() + sDbl);		// �Ǘ��ҋ敪
					sbData.Append(sKanma + sDbl + reader.GetString(24).TrimEnd() + sDbl);		// �^����\���i�L���A�g�e�f�j
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(25).ToString().TrimEnd() + sDbl); // �o�^����
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(26).ToString().TrimEnd() + sDbl); // �X�V����
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� END
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// ����b�c
					sbData.Append(sKanma + sDbl + reader.GetString(5).Trim() + sDbl);			// ���喼
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim() + sDbl);	// �Ǘ��X���b�c
					sbData.Append(sKanma + sDbl + reader.GetString(7).Trim() + sDbl);			// �Ǘ��X����
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� START
//					sbData.Append(sKanma + sDbl + reader.GetString(8).Trim() + sDbl);			// �ݒu��Z���P
//					sbData.Append(sKanma + sDbl + reader.GetString(9).Trim() + sDbl);			// �ݒu��Z���Q
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(8).Trim() + sDbl);	// �ݒu��Z���P
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim() + sDbl);	// �ݒu��Z���Q
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� END
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� START
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);		// Ver.�i�g�D�b�c�j
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);		// �X�֔ԍ�
					sbData.Append(sKanma + sDbl + reader.GetDecimal(29).ToString().TrimEnd() + sDbl); // �g�p��
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl); // ����\���Ǘ��ԍ�
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(31).ToString().TrimEnd() + sDbl); // �o�^����
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(32).ToString().TrimEnd() + sDbl); // �X�V����
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� END
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(10).Trim() + sDbl);	// ���p�҂b�c
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// �p�X���[�h
					sbData.Append(sKanma + sDbl + reader.GetString(12).Trim() + sDbl       );	// ���p�Җ�
					sbData.Append(sKanma + sDbl + reader.GetString(13).Trim() + sDbl);			// ���p�ғo�^��
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� START
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).TrimEnd() + sDbl); // �ב��l�b�c
					sbData.Append(sKanma + sDbl + reader.GetString(34).TrimEnd() + sDbl);		 // ���x������֎~
					sbData.Append(sKanma + sDbl + reader.GetDecimal(35).ToString().TrimEnd() + sDbl); // �F�؃G���[��
					sbData.Append(sKanma + sDbl + reader.GetString(36).TrimEnd() + sDbl); // �p�X���[�h�X�V���i�o�^�o�f�j
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(37).ToString().TrimEnd() + sDbl); // �o�^����
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(38).ToString().TrimEnd() + sDbl); // �X�V����
// MOD 2009.11.25 ���s�j���� ���q�l���o�́i�b�r�u�j�̍��ڒǉ� END
// ADD 2006.12.11 ���s�j�����J �T�[�}���䐔�ǉ� START
					sbData.Append(sKanma + sDbl + reader.GetDecimal(14) + sDbl);			// �T�[�}���䐔
// ADD 2006.12.11 ���s�j�����J �T�[�}���䐔�ǉ� END
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(15).Trim() + sDbl);	// �V���A���ԍ��P
					sbData.Append(sKanma + sDbl + reader.GetString(16).Trim() + sDbl);			// ��ԂP
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(17).Trim() + sDbl);	// �V���A���ԍ��Q
					sbData.Append(sKanma + sDbl + reader.GetString(18).Trim() + sDbl);			// ��ԂQ
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(19).Trim() + sDbl);	// �V���A���ԍ��R
					sbData.Append(sKanma + sDbl + reader.GetString(20).Trim() + sDbl);			// ��ԂR
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(21).Trim() + sDbl);	// �V���A���ԍ��S
					sbData.Append(sKanma + sDbl + reader.GetString(22).Trim() + sDbl);			// ��ԂS
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END

					sList.Add(sbData);
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}


// ADD 2005.05.27 ���s�j���� ���b�Z�[�W�o�^��ʒǉ� START
		/*********************************************************************
		 * �V�X�e���Ǘ��f�[�^�擾
		 * �����F����b�c�A�V�X�e���Ǘ��b�c
		 * �ߒl�F�X�e�[�^�X�A���b�Z�[�W�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Sel_Syskanri(string[] sUser, string sKanCode)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�V�X�e���Ǘ��f�[�^�擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[4];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}

			try
			{
				string cmdQuery
					= "SELECT \"���b�Z�[�W\", TO_CHAR(�X�V����) \n"
					+  " FROM �`�l�O�P�V�X�e���Ǘ� \n"
					+ " WHERE �V�X�e���Ǘ��b�c = '" + sKanCode + "' \n"
					+   " AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				bool bRead = reader.Read();
				if(bRead == true)
				{
					sRet[1] = reader.GetString(0).TrimEnd();
					sRet[2] = reader.GetString(1);
					sRet[0] = "����I��";
				}
				else
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * �L���f�[�^�X�V
		 * �����F����b�c�A�V�X�e���Ǘ��b�c�A���b�Z�[�W
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_Syskanri(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�V�X�e���Ǘ��f�[�^�X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				string cmdQuery 
					= "UPDATE �`�l�O�P�V�X�e���Ǘ� \n"
					+   " SET \"���b�Z�[�W\" = '" + sData[1] +"', \n"
					+       " �폜�e�f   = '0', \n"
					+       " �X�V�o�f   = '" + sData[2] +"', \n"
					+       " �X�V��     = '" + sData[3] +"', \n"
					+       " �X�V����   =  TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')  \n"
					+ " WHERE �V�X�e���Ǘ��b�c = '" + sData[0] +"' \n"
					+   " AND �X�V���� =  " + sData[4] +" \n";

				int iUpdRow = CmdUpdate(sUser, conn2, cmdQuery);

				tran.Commit();
				if(iUpdRow == 0)
					sRet[0] = "�f�[�^�ҏW���ɑ��̒[�����X�V����Ă��܂��B\r\n�ēx�A�ŐV�f�[�^���Ăяo���čX�V���Ă��������B";
				else				
					sRet[0] = "����I��";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}
// ADD 2005.05.27 ���s�j���� ���b�Z�[�W�o�^��ʒǉ� END
// ADD 2006.08.03 ���s�j���� ����\���@�\�̒ǉ� START
		/*********************************************************************
		 * �\�����擾
		 * �����F�Ǘ��ԍ�
		 * �ߒl�F�X�e�[�^�X�A�Ǘ��ԍ��A������A�g�p�J�n���A�Ǘ��ҋ敪�A�g�p�I����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Mosikomi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�\����񌟍��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[43];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �Ǘ��ԍ� "
					+       ",�X���b�c "
					+       ",�\���҃J�i "
					+       ",�\���Җ� "
					+       ",�\���җX�֔ԍ� \n"
					+       ",�\���Ҍ��b�c "
					+       ",�\���ҏZ���P "
					+       ",�\���ҏZ���Q "
					+       ",�\���ғd�b�P "
					+       ",�\���ғd�b�Q \n"
					+       ",�\���ғd�b�R "
					+       ",�\���ғd�b "
					+       ",�\���҂e�`�w�P "
					+       ",�\���҂e�`�w�Q "
					+       ",�\���҂e�`�w�R \n"
					+       ",�ݒu�ꏊ�敪 "
					+       ",�ݒu�ꏊ�J�i "
					+       ",�ݒu�ꏊ�� "
					+       ",�ݒu�ꏊ�X�֔ԍ� "
					+       ",�ݒu�ꏊ���b�c \n"
					+       ",�ݒu�ꏊ�Z���P "
					+       ",�ݒu�ꏊ�Z���Q "
					+       ",�ݒu�ꏊ�d�b�P "
					+       ",�ݒu�ꏊ�d�b�Q "
					+       ",�ݒu�ꏊ�d�b�R \n"
					+       ",�ݒu�ꏊ�e�`�w�P "
					+       ",�ݒu�ꏊ�e�`�w�Q "
					+       ",�ݒu�ꏊ�e�`�w�R "
					+       ",�ݒu�ꏊ�S���Җ� "
					+       ",�ݒu�ꏊ��E�� \n"
					+       ",�ݒu�ꏊ�g�p�� "
					+       ",����b�c "
					+       ",�g�p�J�n�� "
					+       ",����b�c "
					+       ",���喼 \n"
					+       ",\"�T�[�}���䐔\" "
					+       ",���p�҂b�c "
					+       ",���p�Җ� "
					+       ",\"�p�X���[�h\" "
					+       ",���F��Ԃe�f \n"
					+       ",���� "
					+       ",TO_CHAR(�X�V����) "
					+       ",�X�V�� \n"
					+  " FROM �r�l�O�T����\�� \n"
					+ " WHERE �Ǘ��ԍ� = '" + sKey[0] + "' \n"
					+    "AND �폜�e�f = '0' \n";

				if(sKey[1].Trim().Length !=0)
					cmdQuery += "AND �X���b�c = '" + sKey[1] + "' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				if(reader.Read())
				{
					sRet[0] = "����I��";
					//�Ǘ��ԍ��̓_�~�[
					sRet[1] = reader.GetString(1).Trim();
					sRet[2] = reader.GetString(2).Trim();
					sRet[3] = reader.GetString(3).Trim();
					sRet[4] = reader.GetString(4).Trim();
					sRet[5] = reader.GetString(5).Trim();
					sRet[6] = reader.GetString(6).Trim();
					sRet[7] = reader.GetString(7).Trim();
					sRet[8] = reader.GetString(8).Trim();
					sRet[9] = reader.GetString(9).Trim();
					sRet[10] = reader.GetString(10).Trim();
					sRet[11] = reader.GetString(11).Trim();
					sRet[12] = reader.GetString(12).Trim();
					sRet[13] = reader.GetString(13).Trim();
					sRet[14] = reader.GetString(14).Trim();
					sRet[15] = reader.GetString(15).Trim();
					sRet[16] = reader.GetString(16).Trim();
					sRet[17] = reader.GetString(17).Trim();
					sRet[18] = reader.GetString(18).Trim();
					sRet[19] = reader.GetString(19).Trim();
					sRet[20] = reader.GetString(20).Trim();
					sRet[21] = reader.GetString(21).Trim();
					sRet[22] = reader.GetString(22).Trim();
					sRet[23] = reader.GetString(23).Trim();
					sRet[24] = reader.GetString(24).Trim();
					sRet[25] = reader.GetString(25).Trim();
					sRet[26] = reader.GetString(26).Trim();
					sRet[27] = reader.GetString(27).Trim();
					sRet[28] = reader.GetString(28).Trim();
					sRet[29] = reader.GetString(29).Trim();
					sRet[30] = reader.GetDecimal(30).ToString().Trim();
					sRet[31] = reader.GetString(31).Trim();
					sRet[32] = reader.GetString(32).Trim();
					sRet[33] = reader.GetString(33).Trim();
					sRet[34] = reader.GetString(34).Trim();
					sRet[35] = reader.GetDecimal(35).ToString().Trim();
					sRet[36] = reader.GetString(36).Trim();
					sRet[37] = reader.GetString(37).Trim();
					sRet[38] = reader.GetString(38).Trim();
					sRet[39] = reader.GetString(39).Trim();
					sRet[40] = reader.GetString(40).Trim();
					sRet[41] = reader.GetString(41).Trim();
					sRet[42] = reader.GetString(42).Trim();
				}
				else
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �\�����ꗗ�擾
		 * �����F�Ǘ��ԍ��A�����
		 * �ߒl�F�X�e�[�^�X�A�Ǘ��ԍ��A�����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Mosikomi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�\�����ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TO_CHAR(�Ǘ��ԍ�,'0000000') || '|' "
					+     "|| TRIM(�\���Җ�) || '|' "
					+     "|| TRIM(�\���҃J�i) || '|' "
					+     "|| TRIM(�X���b�c) || '|' \n"
					+  " FROM �r�l�O�T����\�� \n";

				bool bWhere = true;
				if (sKey[0].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " �Ǘ��ԍ� = " + sKey[0] + " \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " �X���b�c = '" + sKey[1] + "' \n";
				}
				if (sKey[2].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " �\���Җ� LIKE '%" + sKey[2] + "%' \n";
				}
				if (sKey[3].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " �\���҃J�i LIKE '%" + sKey[3] + "%' \n";
				}
				if(bWhere){ cmdQuery+=" WHERE "; bWhere = false;} else cmdQuery+=" AND";
				cmdQuery += " �폜�e�f = '0' \n";
				cmdQuery += " ORDER BY �Ǘ��ԍ� \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �\�����ǉ�
		 * �����F�Ǘ��ԍ��A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V�����A�Ǘ��ԍ�
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Mosikomi(string[] sUser, string[] sData)
		{
			//�Ǘ��ԍ��̎擾
			string[] sKey   = {" ", sData[42]};	//�X���b�c�A�X�V��
			string[] sKanri = Get_KaniSaiban(sUser, sKey);
			if(sKanri[0].Length > 4)
			{
				return sKanri;
			}
			sData[0] = sKanri[1];

// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�\�����ǉ��J�n");

			OracleConnection conn2 = null;

//			string[] sRet = new string[3]{"","",""};
//			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string[] sRet = new string[3]{"", s�X�V����, sData[0]};

// ADD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
			string s�X�V�o�f = "�\���o�^";
			if(sData.Length > 43)
				s�X�V�o�f = sData[43];
// ADD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f \n"
					+   "FROM �r�l�O�T����\�� \n"
					+  "WHERE �Ǘ��ԍ� = " + sData[0] + " \n"
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				if(reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}

				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �r�l�O�T����\�� \n"
						+ " VALUES ( " + sData[0] + "  " 
						+         ",'" + sData[1] + "' "
						+         ",'" + sData[2] + "' "
						+         ",'" + sData[3] + "' "
						+         ",'" + sData[4] + "' \n"
						+         ",'" + sData[5] + "' "
						+         ",'" + sData[6] + "' "
						+         ",'" + sData[7] + "' "
						+         ",'" + sData[8] + "' "
						+         ",'" + sData[9] + "' \n"
						+         ",'" + sData[10] + "' "
						+         ",'" + sData[11] + "' "
						+         ",'" + sData[12] + "' "
						+         ",'" + sData[13] + "' "
						+         ",'" + sData[14] + "' \n"
						+         ",'" + sData[15] + "' "
						+         ",'" + sData[16] + "' "
						+         ",'" + sData[17] + "' "
						+         ",'" + sData[18] + "' "
						+         ",'" + sData[19] + "' \n"
						+         ",'" + sData[20] + "' "
						+         ",'" + sData[21] + "' "
						+         ",'" + sData[22] + "' "
						+         ",'" + sData[23] + "' "
						+         ",'" + sData[24] + "' \n"
						+         ",'" + sData[25] + "' "
						+         ",'" + sData[26] + "' "
						+         ",'" + sData[27] + "' "
						+         ",'" + sData[28] + "' "
						+         ",'" + sData[29] + "' \n"
						+         ", " + sData[30] + "  "
						+         ",'" + sData[31] + "' "
						+         ",'" + sData[32] + "' "
						+         ",'" + sData[33] + "' "
						+         ",'" + sData[34] + "' \n"
						+         ", " + sData[35] + "  "
						+         ",'" + sData[36] + "' "
						+         ",'" + sData[37] + "' "
						+         ",'" + sData[38] + "' "
						+         ",'" + sData[39] + "' \n"
						+         ",'" + sData[40] + "' \n"
						+         ",'0' \n"
						+         "," + s�X�V����
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
//						+         ",'�\���o�^' "
						+         ",'" + s�X�V�o�f + "' "
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END
						+         ",'" + sData[42] + "' \n"
						+         "," + s�X�V����
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
//						+         ",'�\���o�^' "
						+         ",'" + s�X�V�o�f + "' "
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END
						+         ",'" + sData[42] + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);

					tran.Commit();
					sRet[0] = "����I��";

//					sRet[1] = s�X�V����;
//					sRet[2] = sData[0];		//�Ǘ��ԍ�

				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �r�l�O�T����\�� \n"
							+   " SET �X���b�c = '" + sData[1] + "' \n"
							+       ",�\���҃J�i = '" + sData[2] + "' \n"
							+       ",�\���Җ� = '" + sData[3] + "' \n"
							+       ",�\���җX�֔ԍ� = '" + sData[4] + "' \n"
							+       ",�\���Ҍ��b�c = '" + sData[5] + "' \n"
							+       ",�\���ҏZ���P = '" + sData[6] + "' \n"
							+       ",�\���ҏZ���Q = '" + sData[7] + "' \n"
							+       ",�\���ғd�b�P = '" + sData[8] + "' \n"
							+       ",�\���ғd�b�Q = '" + sData[9] + "' \n"
							+       ",�\���ғd�b�R = '" + sData[10] + "' \n"
							+       ",�\���ғd�b = '" + sData[11] + "' \n"
							+       ",�\���҂e�`�w�P = '" + sData[12] + "' \n"
							+       ",�\���҂e�`�w�Q = '" + sData[13] + "' \n"
							+       ",�\���҂e�`�w�R = '" + sData[14] + "' \n"
							+       ",�ݒu�ꏊ�敪 = '" + sData[15] + "' \n"
							+       ",�ݒu�ꏊ�J�i = '" + sData[16] + "' \n"
							+       ",�ݒu�ꏊ�� = '" + sData[17] + "' \n"
							+       ",�ݒu�ꏊ�X�֔ԍ� = '" + sData[18] + "' \n"
							+       ",�ݒu�ꏊ���b�c = '" + sData[19] + "' \n"
							+       ",�ݒu�ꏊ�Z���P = '" + sData[20] + "' \n"
							+       ",�ݒu�ꏊ�Z���Q = '" + sData[21] + "' \n"
							+       ",�ݒu�ꏊ�d�b�P = '" + sData[22] + "' \n"
							+       ",�ݒu�ꏊ�d�b�Q = '" + sData[23] + "' \n"
							+       ",�ݒu�ꏊ�d�b�R = '" + sData[24] + "' \n"
							+       ",�ݒu�ꏊ�e�`�w�P = '" + sData[25] + "' \n"
							+       ",�ݒu�ꏊ�e�`�w�Q = '" + sData[26] + "' \n"
							+       ",�ݒu�ꏊ�e�`�w�R = '" + sData[27] + "' \n"
							+       ",�ݒu�ꏊ�S���Җ� = '" + sData[28] + "' \n"
							+       ",�ݒu�ꏊ��E�� = '" + sData[29] + "' \n"
							+       ",�ݒu�ꏊ�g�p�� =  " + sData[30] + "  \n"
							+       ",����b�c = '" + sData[31] + "' \n"
							+       ",�g�p�J�n�� = '" + sData[32] + "' \n"
							+       ",����b�c = '" + sData[33] + "' \n"
							+       ",���喼 = '" + sData[34] + "' \n"
							+       ",�T�[�}���䐔 =  " + sData[35] + "  \n"
							+       ",���p�҂b�c = '" + sData[36] + "' \n"
							+       ",���p�Җ� = '" + sData[37] + "' \n"
							+       ",�p�X���[�h = '" + sData[38] + "' \n"
							+       ",���F��Ԃe�f = '" + sData[39] + "' \n"
							+       ",���� = '" + sData[40] + "' \n"
							+       ",�폜�e�f = '0' \n"
							+       ",�o�^���� = " + s�X�V���� + " \n"
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
//							+       ",�o�^�o�f = '�\���o�^' \n"
							+       ",�o�^�o�f = '" + s�X�V�o�f + "' \n"
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END
							+       ",�o�^�� = '" + sData[42] + "' \n"
							+       ",�X�V���� = " + s�X�V���� + " \n"
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� START
//							+       ",�X�V�o�f = '�\���o�^' \n"
							+       ",�X�V�o�f = '" + s�X�V�o�f + "' \n"
// MOD 2007.01.27 ���s�j���� ����\���ւ̒ǉ� END
							+       ",�X�V�� = '" + sData[42] + "' \n"
							+ " WHERE �Ǘ��ԍ� = '" + sData[0] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);

						string sRet���   = "";
						string sRet����   = "";
						string sRet���p�� = "";
						//���F��Ԃe�f��[3�F���F��]�̏ꍇ
						if(sData[39].Equals("3")){
							sRet��� = Ins_Member2(sUser, conn2, sData, s�X�V����);
							if(sRet���.Length == 4){
								//����}�X�^�ǉ�
								sRet���� = Ins_Section2(sUser, conn2, sData, s�X�V����);
								if(sRet����.Length == 4){
									//���p�҃}�X�^�ǉ�
									sRet���p�� = Ins_User2(sUser, conn2, sData, s�X�V����);
								}
							}
						}
						if(sRet���.Length > 4){
							tran.Rollback();
							sRet[0] = "���q�l�F" + sRet���;
						}else if(sRet����.Length > 4){
							tran.Rollback();
							sRet[0] = "�Z�N�V�����F" + sRet����;
						}else if(sRet���p��.Length > 4){
							tran.Rollback();
							sRet[0] = "���[�U�[�F" + sRet���p��;
						}else{
							tran.Commit();
							sRet[0] = "����I��";

//							sRet[1] = s�X�V����;
//							sRet[2] = sData[0];		//�Ǘ��ԍ�

						}
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �\�����X�V
		 * �����F�Ǘ��ԍ��A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
// ADD 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� START
		private static string UPD_MOSIKOMI_SELECT
			= "SELECT �Ǘ��ԍ� "
			+ ", �X���b�c "
			+ ", �\���҃J�i "
			+ ", �\���Җ� "
			+ ", �\���җX�֔ԍ� \n"
			+ ", �\���Ҍ��b�c "
			+ ", �\���ҏZ���P "
			+ ", �\���ҏZ���Q "
			+ ", �\���ғd�b�P "
			+ ", �\���ғd�b�Q \n"
			+ ", �\���ғd�b�R "
			+ ", �\���ғd�b "
			+ ", �\���҂e�`�w�P "
			+ ", �\���҂e�`�w�Q "
			+ ", �\���҂e�`�w�R \n"
			+ ", �ݒu�ꏊ�敪 "
			+ ", �ݒu�ꏊ�J�i "
			+ ", �ݒu�ꏊ�� "
			+ ", �ݒu�ꏊ�X�֔ԍ� "
			+ ", �ݒu�ꏊ���b�c \n"
			+ ", �ݒu�ꏊ�Z���P "
			+ ", �ݒu�ꏊ�Z���Q "
			+ ", �ݒu�ꏊ�d�b�P "
			+ ", �ݒu�ꏊ�d�b�Q "
			+ ", �ݒu�ꏊ�d�b�R \n"
			+ ", �ݒu�ꏊ�e�`�w�P "
			+ ", �ݒu�ꏊ�e�`�w�Q "
			+ ", �ݒu�ꏊ�e�`�w�R "
			+ ", �ݒu�ꏊ�S���Җ� "
			+ ", �ݒu�ꏊ��E�� \n"
			+ ", �ݒu�ꏊ�g�p�� "
			+ ", ����b�c "
			+ ", �g�p�J�n�� "
			+ ", ����b�c "
			+ ", ���喼 \n"
			+ ", \"�T�[�}���䐔\" "
			+ ", ���p�҂b�c "
			+ ", ���p�Җ� "
			+ ", \"�p�X���[�h\" "
			+ ", ���F��Ԃe�f \n"
			+ ", ���� "
			+ ", TO_CHAR(�X�V����) "
			+ ", �X�V�� \n"
			+ "FROM �r�l�O�T����\�� \n"
			+ "";

		private static string UPD_MOSIKOMI_DELETE
			= "UPDATE �r�l�O�T����\�� \n"
			+ "SET �폜�e�f = '1' \n"
			+ "";
// ADD 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� END

		[WebMethod]
		public string[] Upd_Mosikomi(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�\�����X�V�J�n");

			OracleConnection conn2 = null;
// DEL 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� START
//			string[] sRet = new string[2]{"",""};
// DEL 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� END
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");
// ADD 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� START
			string[] sRet = new string[3]{"", s�X�V����, sData[0]};
// ADD 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";

			try
			{
// ADD 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� START
				bool bUpdState = false;

				//���F��Ԃe�f��[1�F�\����]�̏ꍇ�i����{�^���̎��j
				if(sData[39].Equals("1")){
					string[] sRefData = new string[43];
					cmdQuery = UPD_MOSIKOMI_SELECT
							+ " WHERE �Ǘ��ԍ� = '" + sData[0] + "' \n"
							+ " AND �폜�e�f = '0' \n"
							+ " AND �X�V���� = " + sData[41] + " \n"
							+ " FOR UPDATE \n";

					OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
					if(!reader.Read())
					{
						tran.Rollback();
						sRet[0] = "���̒[���ōX�V����Ă��܂�";
						logWriter(sUser, INF, sRet[0]);
						return sRet;
					}
					sRefData[0] = "";
					//�Ǘ��ԍ��̓_�~�[
					sRefData[1] = reader.GetString(1).Trim();
					sRefData[2] = reader.GetString(2).Trim();
					sRefData[3] = reader.GetString(3).Trim();
					sRefData[4] = reader.GetString(4).Trim();
					sRefData[5] = reader.GetString(5).Trim();	//�\���Ҍ��b�c
					sRefData[6] = reader.GetString(6).Trim();
					sRefData[7] = reader.GetString(7).Trim();
					sRefData[8] = reader.GetString(8).Trim();
					sRefData[9] = reader.GetString(9).Trim();
					sRefData[10] = reader.GetString(10).Trim();	//�\���ғd�b�R
					sRefData[11] = reader.GetString(11).Trim();
					sRefData[12] = reader.GetString(12).Trim();
					sRefData[13] = reader.GetString(13).Trim();
					sRefData[14] = reader.GetString(14).Trim();
					sRefData[15] = reader.GetString(15).Trim();	//�ݒu�ꏊ�敪
					sRefData[16] = reader.GetString(16).Trim();
					sRefData[17] = reader.GetString(17).Trim();
					sRefData[18] = reader.GetString(18).Trim();
					sRefData[19] = reader.GetString(19).Trim();
					sRefData[20] = reader.GetString(20).Trim();	//�ݒu�ꏊ�Z���P
					sRefData[21] = reader.GetString(21).Trim();
					sRefData[22] = reader.GetString(22).Trim();
					sRefData[23] = reader.GetString(23).Trim();
					sRefData[24] = reader.GetString(24).Trim();
					sRefData[25] = reader.GetString(25).Trim();	//�ݒu�ꏊ�e�`�w�P
					sRefData[26] = reader.GetString(26).Trim();
					sRefData[27] = reader.GetString(27).Trim();
					sRefData[28] = reader.GetString(28).Trim();
					sRefData[29] = reader.GetString(29).Trim();
					sRefData[30] = reader.GetDecimal(30).ToString().Trim();	//�ݒu�ꏊ�g�p��
					sRefData[31] = reader.GetString(31).Trim();
					sRefData[32] = reader.GetString(32).Trim();
					sRefData[33] = reader.GetString(33).Trim();
					sRefData[34] = reader.GetString(34).Trim();
					sRefData[35] = reader.GetDecimal(35).ToString().Trim();	//�T�[�}���䐔
					sRefData[36] = reader.GetString(36).Trim();
					sRefData[37] = reader.GetString(37).Trim();
					sRefData[38] = reader.GetString(38).Trim();
					sRefData[39] = reader.GetString(39).Trim();
					sRefData[40] = reader.GetString(40).Trim();	//����
					sRefData[41] = reader.GetString(41).Trim();
					sRefData[42] = reader.GetString(42).Trim();

					//���F��Ԃe�f�i_:�o�^���A1:�\�����A2:���ے��A3:���F�ρj��
					//�i1:�\������������2:���ے��̂��́j
					if(sRefData[39].Length > 0){
						//�f�[�^�̍X�V�󋵂��`�F�b�N����
						for(int iCnt = 2; iCnt <= 30; iCnt++){
							if(!sRefData[iCnt].Equals(sData[iCnt].Trim())){
								bUpdState = true;
								break;
							}
						}

						if(bUpdState){
							//�f�[�^�폜
							cmdQuery = UPD_MOSIKOMI_DELETE
							+ ", �X�V�o�f = '�\���X�V' \n"
							+ ", �X�V��   = '" + sData[42] +"' \n"
							+ ", �X�V���� = "+ s�X�V���� + " \n"
							+ " WHERE �Ǘ��ԍ� = '" + sData[0] + "' \n"
							+ " AND �폜�e�f = '0' \n"
							+ " AND �X�V���� = " + sData[41] + " \n";

							if (CmdUpdate(sUser, conn2, cmdQuery) == 0)
							{
								tran.Rollback();
								sRet[0] = "���̒[���ōX�V����Ă��܂�";
							}else{
								tran.Commit();
								sRet[0] = "����I��";
							}
							logWriter(sUser, INF, sRet[0]);
							//�f�[�^���ύX����Ă���ꍇ�ɂ́A�V�����󒍂m�n�Ńf�[�^��ǉ�����
//�ۗ��@�g�����U�N�V��������
							return Ins_Mosikomi(sUser, sData);
						}
					}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				}
// ADD 2007.01.30 ���s�j���� ����ρi�\�����j�ȍ~�̏ꍇ�ɂ́A�V������t�m�n���̔� END

				cmdQuery
					= "UPDATE �r�l�O�T����\�� \n"
					+   " SET �X���b�c = '" + sData[1] + "' \n"
					+       ",�\���҃J�i = '" + sData[2] + "' \n"
					+       ",�\���Җ� = '" + sData[3] + "' \n"
					+       ",�\���җX�֔ԍ� = '" + sData[4] + "' \n"
					+       ",�\���Ҍ��b�c = '" + sData[5] + "' \n"
					+       ",�\���ҏZ���P = '" + sData[6] + "' \n"
					+       ",�\���ҏZ���Q = '" + sData[7] + "' \n"
					+       ",�\���ғd�b�P = '" + sData[8] + "' \n"
					+       ",�\���ғd�b�Q = '" + sData[9] + "' \n"
					+       ",�\���ғd�b�R = '" + sData[10] + "' \n"
					+       ",�\���ғd�b = '" + sData[11] + "' \n"
					+       ",�\���҂e�`�w�P = '" + sData[12] + "' \n"
					+       ",�\���҂e�`�w�Q = '" + sData[13] + "' \n"
					+       ",�\���҂e�`�w�R = '" + sData[14] + "' \n"
					+       ",�ݒu�ꏊ�敪 = '" + sData[15] + "' \n"
					+       ",�ݒu�ꏊ�J�i = '" + sData[16] + "' \n"
					+       ",�ݒu�ꏊ�� = '" + sData[17] + "' \n"
					+       ",�ݒu�ꏊ�X�֔ԍ� = '" + sData[18] + "' \n"
					+       ",�ݒu�ꏊ���b�c = '" + sData[19] + "' \n"
					+       ",�ݒu�ꏊ�Z���P = '" + sData[20] + "' \n"
					+       ",�ݒu�ꏊ�Z���Q = '" + sData[21] + "' \n"
					+       ",�ݒu�ꏊ�d�b�P = '" + sData[22] + "' \n"
					+       ",�ݒu�ꏊ�d�b�Q = '" + sData[23] + "' \n"
					+       ",�ݒu�ꏊ�d�b�R = '" + sData[24] + "' \n"
					+       ",�ݒu�ꏊ�e�`�w�P = '" + sData[25] + "' \n"
					+       ",�ݒu�ꏊ�e�`�w�Q = '" + sData[26] + "' \n"
					+       ",�ݒu�ꏊ�e�`�w�R = '" + sData[27] + "' \n"
					+       ",�ݒu�ꏊ�S���Җ� = '" + sData[28] + "' \n"
					+       ",�ݒu�ꏊ��E�� = '" + sData[29] + "' \n"
					+       ",�ݒu�ꏊ�g�p�� =  " + sData[30] + "  \n"
					+       ",����b�c = '" + sData[31] + "' \n"
					+       ",�g�p�J�n�� = '" + sData[32] + "' \n"
					+       ",����b�c = '" + sData[33] + "' \n"
					+       ",���喼 = '" + sData[34] + "' \n"
					+       ",�T�[�}���䐔 =  " + sData[35] + "  \n"
					+       ",���p�҂b�c = '" + sData[36] + "' \n"
					+       ",���p�Җ� = '" + sData[37] + "' \n"
					+       ",�p�X���[�h = '" + sData[38] + "' \n"
					+       ",���F��Ԃe�f = '" + sData[39] + "' \n"
					+       ",���� = '" + sData[40] + "' \n"
					+       ",�X�V���� = " + s�X�V���� + " \n"
					+       ",�X�V�o�f = '�\���X�V' \n"
					+       ",�X�V�� = '" + sData[42] + "' \n"
					+ " WHERE �Ǘ��ԍ� = '" + sData[0] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� = " + sData[41] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					string sRet���   = "";
					string sRet����   = "";
					string sRet���p�� = "";
					//���F��Ԃe�f��[3�F���F��]�̏ꍇ
					if(sData[39].Equals("3")){
						sRet��� = Ins_Member2(sUser, conn2, sData, s�X�V����);
						if(sRet���.Length == 4){
							//����}�X�^�ǉ�
							sRet���� = Ins_Section2(sUser, conn2, sData, s�X�V����);
							if(sRet����.Length == 4){
								//���p�҃}�X�^�ǉ�
								sRet���p�� = Ins_User2(sUser, conn2, sData, s�X�V����);
							}
						}
					}
					if(sRet���.Length > 4){
						tran.Rollback();
						sRet[0] = "���q�l�F" + sRet���;
					}else if(sRet����.Length > 4){
						tran.Rollback();
						sRet[0] = "�Z�N�V�����F" + sRet����;
					}else if(sRet���p��.Length > 4){
						tran.Rollback();
						sRet[0] = "���[�U�[�F" + sRet���p��;
					}else{
						tran.Commit();
						sRet[0] = "����I��";
						sRet[1] = s�X�V����;
					}
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
				logWriter(sUser, ERR, "StackTrace:\n" + ex.StackTrace);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �\�����폜
		 * �����F�Ǘ��ԍ�
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Mosikomi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�\�����폜�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �r�l�O�T����\�� \n"
					+    "SET �폜�e�f = '1' "
					+       ",�X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '�\���폜' "
					+       ",�X�V�� = '" + sKey[2] + "' \n"
					+ " WHERE �Ǘ��ԍ� = '" + sKey[0] + "' "
					+   " AND �X�V���� = " + sKey[1] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �Ǘ��ԍ��̍̔�
		 * �����F����b�c�A����b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Get_KaniSaiban(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�Ǘ��ԍ��̎擾�J�n");
			
			OracleConnection conn2 = null;
			string[] sRet = new string[2]{"",""};
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			//�g�����U�N�V�����̐ݒ�
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				decimal i�J�����g�ԍ� = 0;
				decimal i�J�n�ԍ�     = 0;
				decimal i�I���ԍ�     = 0;

				string cmdQuery
					= "SELECT �J�����g�ԍ�, �J�n�ԍ�, �I���ԍ� \n"
					+ " FROM �b�l�P�U�X���̔ԊǗ� \n"
					+ " WHERE �̔ԋ敪 = '01' \n"
					+ " AND �X���b�c = '" + sKey[0] + "' \n"
					+ " FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				string updQuery = "";
				if(reader.Read())
				{
					i�J�����g�ԍ� = reader.GetDecimal(0);
					i�J�n�ԍ�     = reader.GetDecimal(1);
					i�I���ԍ�     = reader.GetDecimal(2);

					if(i�J�����g�ԍ� < i�I���ԍ�)
					{
						i�J�����g�ԍ�++;
					}else{
						i�J�����g�ԍ� = i�J�n�ԍ�;
					}
					sRet[1] = i�J�����g�ԍ�.ToString("0000000");

					updQuery 
						= "UPDATE �b�l�P�U�X���̔ԊǗ� SET \n"
						+ "  �J�����g�ԍ� = " + i�J�����g�ԍ� + " \n"
						+ ", �J�n�ԍ� = " + i�J�n�ԍ� + " \n"
						+ ", �I���ԍ� = " + i�I���ԍ� + " \n"
						+ ", �X�V���� = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') \n"
						+ ", �X�V�o�f = '����\��' \n"
						+ ", �X�V�� = '" + sKey[1] + "' \n"
						+ " WHERE �̔ԋ敪 = '01' \n"
						+ " AND �X���b�c = '" + sKey[0] + "' \n"
						+ " AND �폜�e�f = '0' \n";
				}else{
// MOD 2006.11.30 ���s�j���� ��ʂ̒��� START
//					i�J�����g�ԍ� = 1;
//					i�J�n�ԍ�     = 1;
					i�J�����g�ԍ� = 5005001;
					i�J�n�ԍ�     = 1000001;
// MOD 2006.11.30 ���s�j���� ��ʂ̒��� END
					i�I���ԍ�     = 9999999;
					sRet[1] = i�J�����g�ԍ�.ToString("0000000");

					// �����̔Ԃ̒ǉ�
					updQuery 
						= "INSERT INTO �b�l�P�U�X���̔ԊǗ� VALUES( \n"
						+ " '01' \n"
						+ ",'" + sKey[0] + "' \n"
						+ ", " + i�J�����g�ԍ� + " \n"
						+ ", " + i�J�n�ԍ� + " \n"
						+ ", " + i�I���ԍ� + " \n"
						+ ",'0' \n"
						+ ", TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+ ",'����\��' "
						+ ",'" + sKey[1] + "' \n"
						+ ", TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+ ",'����\��' "
						+ ",'" + sKey[1] + "' \n"
						+ ") \n";
				}
				CmdUpdate(sUser, conn2, updQuery);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				tran.Commit();
				sRet[0] = "����I��";
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ǉ��Q
		 * �����F����b�c�A�����...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		private string Ins_Member2(string[] sUser, OracleConnection conn2, 
												string[] sData, string sUpdateTime)
		{
			//����}�X�^�ǉ�
			string[] sKey = new string[4]{
				sData[31],	//����b�c
				sData[3],	//�\���Җ�
				sData[32],	//�g�p�J�n��
				sData[42]	//�o�^�ҁA�X�V��
			};

			string sRet = "";

			string cmdQuery = "";
			cmdQuery
				= "SELECT �폜�e�f \n"
				+   "FROM �b�l�O�P��� \n"
				+  "WHERE ����b�c = '" + sKey[0] + "' \n"
				+    "FOR UPDATE \n";

			OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
			int iCnt = 1;
			string s�폜�e�f = "";
			while (reader.Read())
			{
				s�폜�e�f = reader.GetString(0);
				iCnt++;
			}
			if(iCnt == 1)
			{
				//�ǉ�
				cmdQuery
					= "INSERT INTO �b�l�O�P��� \n"
					+ " VALUES ('" + sKey[0] + "' "		//����b�c
					+         ",'" + sKey[1] + "' "		//�����
					+         ",'" + sKey[2] + "' "		//�g�p�J�n��
					+         ",'99999999' "			//�g�p�I����
					+         ",'0' \n"					//�Ǘ��ҋ敪
					+         ",'0' "
					+         ",'0' "
					+         ",'0' "
					+         ",'0' "
					+         ",'0' \n"
					+         ",'0' "
					+         ",'0' "
					+         ",' ' "
					+         ", 0 "
					+         ", 0 \n"
					+         ", 0 "
					+         ", 0 "
					+         ", 0 \n"
					+         ",'0' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[3] + "' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[3] + "' \n"
					+ " ) \n";

				CmdUpdate(sUser, conn2, cmdQuery);

				sRet = "����I��";
			}
			else
			{
				//�ǉ��X�V
				if (s�폜�e�f.Equals("1"))
				{
					cmdQuery
						= "UPDATE �b�l�O�P��� \n"
						+   " SET ����� = '" + sKey[1] + "' \n"
						+       ",�g�p�J�n�� = '" + sKey[2] + "' \n"
						+       ",�g�p�I���� = '99999999' \n"
						+       ",�Ǘ��ҋ敪 = '0' \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
						+       ",�L���A�g�e�f = '0' \n"
						+       ",�ۗ�����e�f = '0' \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
						+       ",�폜�e�f = '0' \n"
						+       ",�o�^���� = " + sUpdateTime
						+       ",�o�^�o�f = '����o�^' "
						+       ",�o�^�� = '" + sKey[3] + "' \n"
						+       ",�X�V���� = " + sUpdateTime
						+       ",�X�V�o�f = '����o�^' "
						+       ",�X�V�� = '" + sKey[3] + "' \n"
						+ " WHERE ����b�c = '" + sKey[0] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);

					sRet = "����I��";
				}
				else
				{
					sRet = "���ɓo�^����Ă��܂�";
				}
			}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
			disposeReader(reader);
			reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
//			logWriter(sUser, INF, sRet);

			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ǉ��Q
		 * �����F����b�c�A����b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		private string Ins_Section2(string[] sUser, OracleConnection conn2, 
											string[] sData, string sUpdateTime)
		{
// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//			string[] sKey = new string[8]{
			string[] sKey = new string[10]{
// MOD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
				sData[31],	//����b�c
				sData[33],	//����b�c
				sData[34],	//���喼
				sData[18],	//�ݒu�ꏊ�X�֔ԍ�
				sData[20],	//�ݒu�ꏊ�Z���P
				sData[21],	//�ݒu�ꏊ�Z���Q
				sData[35],	//�T�[�}���䐔
				sData[42]	//�o�^�ҁA�X�V��
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
				,sData[30]	//�ݒu�ꏊ�g�p��
				,sData[0]	//�Ǘ��ԍ�
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
			};
			string sRet = "";

			string cmdQuery = "";

			cmdQuery
				= "SELECT �폜�e�f \n"
				+   "FROM �b�l�O�Q���� \n"
				+  "WHERE ����b�c = '" + sKey[0] + "' \n"
				+    "AND ����b�c = '" + sKey[1] + "' \n"
				+    "FOR UPDATE \n";

			OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
			int iCnt = 1;
			string s�폜�e�f = "";
			while (reader.Read())
			{
				s�폜�e�f = reader.GetString(0);
				iCnt++;
			}
			if(iCnt == 1)
			{
				//�ǉ�
				cmdQuery
					= "INSERT INTO �b�l�O�Q���� \n"
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
						+         "(����b�c \n"
						+         ",����b�c \n"
						+         ",���喼 \n"
						+         ",�g�D�b�c \n"
						+         ",�o�͏� \n"
						+         ",�X�֔ԍ� \n"
						+         ",\"�W���[�i���m�n�o�^��\" \n"
						+         ",\"�W���[�i���m�n�Ǘ�\" \n"
						+         ",���^�m�n \n"
						+         ",�o�ד� \n"
						+         ",�ݒu��Z���P \n"
						+         ",�ݒu��Z���Q \n"
						+         ",�T�[�}���䐔 \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//						+         ",�g�p�� \n"
//						+         ",����\���Ǘ��ԍ� \n"
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
						+         ",�폜�e�f \n"
						+         ",�o�^���� \n"
						+         ",�o�^�o�f \n"
						+         ",�o�^�� \n"
						+         ",�X�V���� \n"
						+         ",�X�V�o�f \n"
						+         ",�X�V�� \n"
						+         ") \n"
// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
					+ " VALUES ('" + sKey[0] + "' "				//����b�c
					+         ",'" + sKey[1] + "' "				//����b�c
					+         ",'" + sKey[2] + "' "				//���喼
					+         ",' ' "							//�g�D�b�c
					+         ", 0 \n"							//�o�͏�
					+         ",'" + sKey[3] + "' "				//�X�֔ԍ�
					+         ",TO_CHAR(SYSDATE,'YYYYMMDD') "	//�W���[�i���m�n�o�^��
					+         ", 0 "							//�W���[�i���Ǘ��m�n
					+         ", 0 "							//���^�m�n
					+         ",TO_CHAR(SYSDATE,'YYYYMMDD') \n"	//�o�ד�
					+         ",'" + sKey[4] + "' "				//�ݒu��Z���P
					+         ",'" + sKey[5] + "' "				//�ݒu��Z���Q
					+         ", " + sKey[6] + " \n"			//�T�[�}���䐔
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//					+         ", " + sKey[8] + " \n"			//�g�p��
//					+         ", " + sKey[9] + " \n"			//����\���Ǘ��ԍ�
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					+         ",'0' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[7] + "' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[7] + "' \n"
					+ " ) \n";

				CmdUpdate(sUser, conn2, cmdQuery);

// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
				cmdQuery
					= "INSERT INTO �b�l�O�U����g�� \n"
					+         "(����b�c \n"
					+         ",����b�c \n"
					+         ",�g�p�� \n"
					+         ",����\���Ǘ��ԍ� \n"
					+         ",�폜�e�f \n"
					+         ",�o�^���� \n"
					+         ",�o�^�o�f \n"
					+         ",�o�^�� \n"
					+         ",�X�V���� \n"
					+         ",�X�V�o�f \n"
					+         ",�X�V�� \n"
					+         ") \n"
					+ " VALUES ('" + sKey[0] + "' "				//����b�c
					+         ",'" + sKey[1] + "' "				//����b�c
					+         ", " + sKey[8] + " \n"			//�g�p��
					+         ", " + sKey[9] + " \n"			//����\���Ǘ��ԍ�
					+         ",'0' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[7] + "' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[7] + "' \n"
					+ " ) \n";
				CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END

				sRet = "����I��";
			}
			else
			{
				//�ǉ��X�V
				if (s�폜�e�f.Equals("1"))
				{
					cmdQuery
						= "UPDATE �b�l�O�Q���� \n"
						+   " SET ���喼 = '" + sKey[2] + "' \n"
						+       ",�g�D�b�c = ' ' \n"
						+       ",�o�͏� = 0 \n"
						+       ",�X�֔ԍ� = '" + sKey[3] + "' \n"
						+       ",�W���[�i���m�n�o�^�� = TO_CHAR(SYSDATE,'YYYYMMDD') \n"
						+       ",�W���[�i���m�n�Ǘ� = 0 \n"
						+       ",���^�m�n = 0 \n"
						+       ",�o�ד� = TO_CHAR(SYSDATE,'YYYYMMDD') \n"
						+       ",�ݒu��Z���P = '" + sKey[4] + "' \n"
						+       ",�ݒu��Z���Q = '" + sKey[5] + "' \n"
						+       ",�T�[�}���䐔 =  " + sKey[6] + " \n"
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� START
//						+       ",�g�p�� = " + sKey[8] + " \n"
//						+       ",����\���Ǘ��ԍ� = " + sKey[9] + " \n"
//// ADD 2009.03.03 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ� END
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
						+       ",�폜�e�f = '0' \n"
						+       ",�o�^���� = " + sUpdateTime
						+       ",�o�^�o�f = '����o�^' "
						+       ",�o�^�� = '" + sKey[7] + "' \n"
						+       ",�X�V���� = " + sUpdateTime
						+       ",�X�V�o�f = '����o�^' "
						+       ",�X�V�� = '" + sKey[7] + "'\n"
						+ " WHERE ����b�c = '" + sKey[0] + "' \n"
						+   " AND ����b�c = '" + sKey[1] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j START
					cmdQuery
						= "UPDATE �b�l�O�U����g�� SET \n"
						+       " �g�p�� = " + sKey[8] + " \n"
						+       ",����\���Ǘ��ԍ� = " + sKey[9] + " \n"
						+       ",�폜�e�f = '0' \n"
						+       ",�o�^���� = " + sUpdateTime
						+       ",�o�^�o�f = '����o�^' "
						+       ",�o�^�� = '" + sKey[7] + "' \n"
						+       ",�X�V���� = " + sUpdateTime
						+       ",�X�V�o�f = '����o�^' "
						+       ",�X�V�� = '" + sKey[7] + "'\n"
						+ " WHERE ����b�c = '" + sKey[0] + "' \n"
						+   " AND ����b�c = '" + sKey[1] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 ���s�j���� �T�[�}���V���A���ԍ��̒ǉ��i�b�l�O�U����g���j END
					sRet = "����I��";
				}
				else
				{
					sRet = "���ɓo�^����Ă��܂�";
				}
			}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
			disposeReader(reader);
			reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
//			logWriter(sUser, INF, sRet);

			//�G���[���́A�I��
			if (!sRet.Equals("����I��")) return sRet;

			logWriter(sUser, INF, "�L���̏����f�[�^�o�^�J�n");

			//�L���̏����f�[�^�̌���
			cmdQuery
				= "SELECT �L���b�c \n"
				+      ", �L�� \n"
				+   "FROM �r�l�O�R�L�� \n"
				+  "WHERE ����b�c = 'default' \n"
				+    "AND ����b�c = '0000' \n"
				+    "AND �폜�e�f = '0' \n";

			OracleDataReader readerDef = CmdSelect(sUser, conn2, cmdQuery);
			string s�����L���b�c = "";
			string s�����L��     = "";
			while (readerDef.Read())
			{
				s�����L���b�c = readerDef.GetString(0);
				s�����L��     = readerDef.GetString(1);

				//�L���̌���
				cmdQuery
					= "SELECT �L���b�c \n"
					+   "FROM �r�l�O�R�L�� \n"
					+  "WHERE ����b�c = '" + sKey[0] + "' \n"
					+    "AND ����b�c = '" + sKey[1] + "' \n"
					+    "AND �L���b�c = '" + s�����L���b�c + "' \n"
				    +    "FOR UPDATE \n";

				OracleDataReader readerNote = CmdSelect(sUser, conn2, cmdQuery);
				if (readerNote.Read())
				{
					//���ɋL��������ꍇ�͐V�K�X�V
					cmdQuery
						= "UPDATE �r�l�O�R�L�� \n"
						+   " SET �L�� = '" + s�����L�� + "' \n"
						+       ",�폜�e�f = '0' \n"
						+       ",�o�^���� = " + sUpdateTime
						+       ",�o�^�o�f = '�����L��' \n"
						+       ",�o�^�� = '" + sKey[7] + "' \n"
						+       ",�X�V���� = " + sUpdateTime
						+       ",�X�V�o�f = '�����L��' \n"
						+       ",�X�V�� = '" + sKey[7] + "' \n"
						+ " WHERE ����b�c = '" + sKey[0] + "' \n"
						+   " AND ����b�c = '" + sKey[1] + "' \n"
						+   " AND �L���b�c = '" + s�����L���b�c + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					sRet = "����I��";
				}
				else
				{
					//�V�K�ǉ�
					cmdQuery
						= "INSERT INTO �r�l�O�R�L�� \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + s�����L���b�c + "' "
						+         ",'" + s�����L�� + "' \n"
						+         ",'0' \n"
						+         "," + sUpdateTime
						+         ",'�����L��' "
						+         ",'" + sKey[7] + "' \n"
						+         "," + sUpdateTime
						+         ",'�����L��' "
						+         ",'" + sKey[7] + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					sRet = "����I��";
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(readerNote);
				readerNote = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
//				logWriter(sUser, INF, sRet);
			}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
			disposeReader(readerDef);
			readerDef = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

			return sRet;
		}

		/*********************************************************************
		 * ���p�҃}�X�^�ǉ��Q
		 * �����F����b�c�A���p�҂b�c�A���p�Җ�
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		private string Ins_User2(string[] sUser, OracleConnection conn2, 
											string[] sData, string sUpdateTime)
		{
			string[] sKey = new string[6]{
				sData[31],	//����b�c
				sData[36],	//���p�҂b�c
				sData[38],	//�p�X���[�h
				sData[37],	//���p�Җ�
				sData[33],	//����b�c
				sData[42]	//�o�^�ҁA�X�V��
			};
			string sRet = "";

			string cmdQuery = "";

			cmdQuery
				= "SELECT �폜�e�f \n"
				+   "FROM �b�l�O�S���p�� \n"
				+  "WHERE ����b�c = '" + sKey[0] + "' \n"
				+    "AND ���p�҂b�c = '" + sKey[1] + "' \n"
				+    "FOR UPDATE \n";

			OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
			int iCnt = 1;
			string s�폜�e�f = "";
			while (reader.Read())
			{
				s�폜�e�f = reader.GetString(0);
				iCnt++;
			}
			if(iCnt == 1)
			{
				//�ǉ�
				cmdQuery
					= "INSERT INTO �b�l�O�S���p�� \n"
					+ " VALUES ('" + sKey[0] + "' "		//����b�c
					+         ",'" + sKey[1] + "' "		//���p�҂b�c
					+         ",'" + sKey[2] + "' "		//�p�X���[�h
					+         ",'" + sKey[3] + "' "		//���p�Җ�
					+         ",'" + sKey[4] + "' \n"	//����b�c
					+         ",' ' "					//�ב��l�b�c
					+         ",0 "						//�F�؃G���[��
					+         ",' ' "					//�����P
					+         ",' ' "
					+         ",' ' \n"
					+         ",' ' "
					+         ",' ' "
					+         ",' ' "
					+         ",' ' "
					+         ",' ' \n"
					+         ",' ' "
					+         ",' ' \n"
					+         ",'0' \n"
					+         "," + sUpdateTime
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� START
//					+         ",'����o�^' "
					+         ",'"+ sUpdateTime.Substring(0,8) +"' "
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� END
					+         ",'" + sKey[5] + "' \n"
					+         "," + sUpdateTime
					+         ",'����o�^' "
					+         ",'" + sKey[5] + "' \n"
					+ " ) \n";

				CmdUpdate(sUser, conn2, cmdQuery);
				sRet = "����I��";
			}
			else
			{
				//�ǉ��X�V
				if (s�폜�e�f.Equals("1"))
				{
					cmdQuery
						= "UPDATE �b�l�O�S���p�� \n"
						+   " SET �p�X���[�h = '" + sKey[2] + "' \n"
						+       ",���p�Җ� = '" + sKey[3] + "' \n"
						+       ",����b�c = '" + sKey[4] + "' \n"
						+       ",�ב��l�b�c = ' ' \n"
						+       ",�F�؃G���[�� = 0 \n"
						+       ",�����P = ' ' \n"
						+       ",�폜�e�f = '0' \n"
						+       ",�o�^���� = " + sUpdateTime
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� START
//						+       ",�o�^�o�f = '����o�^' "
						+       ",�o�^�o�f = '"+ sUpdateTime.Substring(0,8) +"' "
// MOD 2008.05.21 ���s�j���� ���O�C���G���[�񐔂��T��ɂ��� END
						+       ",�o�^�� = '" + sKey[5] + "' \n"
						+       ",�X�V���� = " + sUpdateTime
						+       ",�X�V�o�f = '����o�^' "
						+       ",�X�V�� = '" + sKey[5] + "' \n"
						+ " WHERE ����b�c = '" + sKey[0] + "' \n"
						+   " AND ���p�҂b�c = '" + sKey[1] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					sRet = "����I��";
				}
				else
				{
					sRet = "���ɓo�^����Ă��܂�";
				}
			}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
			disposeReader(reader);
			reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
//			logWriter(sUser, INF, sRet);

			return sRet;
		}
// ADD 2006.08.03 ���s�j���� ����\���@�\�̒ǉ� END  
// ADD 2006.08.22 ���s�j�R�{ �����X�����̎擾�̒ǉ� START  
		/*********************************************************************
		 * �X�֔ԍ��}�X�^�擾
		 * �����F�X�֔ԍ�
		 * �ߒl�F�X�e�[�^�X�A�Z���A�X���������A�X���b�c
		 *
		 * �Q�ƌ��F�������.cs		[]	
		 * �Q�ƌ��F�X�����.cs		[]	
		 * �Q�ƌ��F������}�X�^.cs	[]	
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Postcode1(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X�֔ԍ��}�X�^�����J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[5];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT NVL(CM10.�X����, ' '), \n"
					+ " TRIM(CM14.�s���{����) || TRIM(CM14.�s�撬����) || TRIM(CM14.���於),TRIM(CM10.�X��������),TRIM(CM10.�X���b�c) \n"
//�ۗ��F���q			+ " TRIM(CM14.�s���{����) || TRIM(CM14.�s�撬����) || TRIM(CM14.���於),NVL(TRIM(CM10.�X��������), ' '),TRIM(CM14.�X���b�c) \n"
					+  " FROM �b�l�P�S�X�֔ԍ� CM14 \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
					+    " ON CM14.�X���b�c = CM10.�X���b�c "
					+    "AND CM10.�폜�e�f = '0' \n"
					+ " WHERE CM14.�X�֔ԍ� = '" + sKey[0] + "' \n"
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+   " AND CM14.�폜�e�f = '0' \n";
//���� MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					;

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.08.22 ���s�j�R�{ �����X�����̎擾�̒ǉ� END  


// ADD 2006.09.06 ���s�j�R�{ �_���t�X�����̎擾�̒ǉ� START  
		/*********************************************************************
		 * �X���}�X�^�擾
		 * �����F
		 * �ߒl�F�X�e�[�^�X�A�X����
		 *
		 *********************************************************************/
// ADD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� START
		private static string GET_SHOP_INF_SELECT
			= "SELECT �_�񏑓X������ \n"
			+       ",�_�񏑏Z���s���{�� \n"
			+       ",�_�񏑏Z���P \n"
			+       ",�_�񏑏Z���Q \n"
			+       ",�_�񏑗X�֔ԍ� \n"
			+       ",�_�񏑓d�b�ԍ� \n"
			+       ",�_�񏑂e�`�w�ԍ� \n"
			+       ",�n��P \n"
			+       ",�n��Q \n"
			+       ",TO_CHAR(�X�V����) \n"
			+       " FROM �b�l�P�O�X�� \n";
// ADD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� END
		[WebMethod]
		public string[] Get_ShopInf(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X���}�X�^�����J�n");

			OracleConnection conn2 = null;
// MOD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� START
//			string[] sRet = new string[8];
			string[] sRet = new string[11];
// MOD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� END

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
// MOD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� START
//				cmdQuery
//					= "SELECT CM10.�_�񏑓X������, \n"
//                                      +  "      CM10.�_�񏑏Z���s���{�� , \n"
//                                      +  "      CM10.�_�񏑏Z���P , \n"
//                                      +  "      CM10.�_�񏑏Z���Q , \n"
//                                      +  "      CM10.�_�񏑗X�֔ԍ� , \n"
//                                      +  "      CM10.�_�񏑓d�b�ԍ� , \n"
//                                      +  "      CM10.�_�񏑂e�`�w�ԍ� \n"
//					+  " FROM �b�l�P�O�X�� CM10 \n"
//					+ " WHERE CM10.�X���b�c = '" + sKey[0] + "' \n"
//					+    "AND CM10.�폜�e�f = '0' \n";
				cmdQuery
					= GET_SHOP_INF_SELECT
					+ " WHERE �X���b�c = '" + sKey[0] + "' \n"
					+    "AND �폜�e�f = '0' \n";
// MOD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetString(6).Trim();
// ADD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� START
					sRet[8] = reader.GetString(7).Trim();
					sRet[9] = reader.GetString(8).Trim();
					sRet[10] = reader.GetString(9).Trim();
// ADD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� END
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.09.06 ���s�j�R�{ �_���t�X�����̎擾�̒ǉ� END  

		// MOD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� START
		/*********************************************************************
		 * �X�����X�V
		 * �����F�Ǘ��ԍ��A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_ShopInf(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�X�����X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[2]{"",""};
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
// MOD 2014.09.10 BEVAS)�O�c �x�X�~�ߋ@�\�ǉ� START
				string s���ʎx�X�~�߂e�f;
				string s���q�x�X�~�߂e�f;
				if (sData.Length > 12) 
				{
					s���ʎx�X�~�߂e�f = sData[12];
					s���q�x�X�~�߂e�f = sData[13];
				} 
				else 
				{
					s���ʎx�X�~�߂e�f = "0";
					s���q�x�X�~�߂e�f = "0";
				}
				cmdQuery
					= "UPDATE �b�l�P�O�X�� \n"
					+   " SET �_�񏑓X������ = '" + sData[1] + "' \n"
					+       ",�_�񏑏Z���s���{�� = '" + sData[2] + "' \n"
					+       ",�_�񏑏Z���P = '" + sData[3] + "' \n"
					+       ",�_�񏑏Z���Q = '" + sData[4] + "' \n"
					+       ",�_�񏑗X�֔ԍ� = '" + sData[5] + "' \n"
					+       ",�_�񏑓d�b�ԍ� = '" + sData[6] + "' \n"
					+       ",�_�񏑂e�`�w�ԍ� = '" + sData[7] + "' \n"
					+       ",�n��P = '" + sData[8] + "' \n"
					+       ",�n��Q = '" + sData[9] + "' \n"
					+       ",�X�V���� = " + s�X�V���� + " \n"
					+       ",�X�V�o�f = '�X���X�V' \n"
					+       ",�X�V�� = '" + sData[11] + "' \n"
					+       ",�x�X�~�߂e�f�P = '" + s���ʎx�X�~�߂e�f + "' \n"
					+       ",�x�X�~�߂e�f�Q = '" + s���q�x�X�~�߂e�f + "' \n"
					+ " WHERE �X���b�c = '" + sData[0] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� = " + sData[10] + " \n";
// MOD 2014.09.10 BEVAS)�O�c �x�X�~�ߋ@�\�ǉ� END

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
					sRet[1] = s�X�V����;
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// MOD 2006.11.06 ���s�j���� �X������ʂ̒ǉ� END
// ADD 2006.11.08 ���s�j�R�{ ����}�X�^��X���R�[�h�ōi�荞�� START
		/*********************************************************************
		 * ����}�X�^�擾
		 * �����F����b�c
		 * �ߒl�F�X�e�[�^�X�A����b�c�A������A�g�p�J�n���A�Ǘ��ҋ敪�A�g�p�I����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_MemberTn(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�����J�n");

			OracleConnection conn2 = null;
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
//			string[] sRet = new string[7];
			string[] sRet = new string[8];
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM01.����b�c "
					+       ",CM01.����� "
					+       ",CM01.�g�p�J�n�� "
					+       ",CM01.�Ǘ��ҋ敪 "
					+       ",CM01.�g�p�I���� "
					+       ",CM01.�X�V���� \n"
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
					+       ",CM01.�L���A�g�e�f \n"
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END
					+  " FROM �b�l�O�P��� CM01\n"
					+  "     ,�b�l�O�Q���� CM02\n"
					+  "     ,�b�l�P�S�X�֔ԍ� CM14\n"
					+ " WHERE CM01.����b�c = '" + sKey[0] + "' \n"
					+    "AND CM01.�폜�e�f = '0' \n"
					+    "AND CM01.����b�c = CM02.����b�c \n"
					+    "AND CM02.�폜�e�f = '0' \n"
					+    "AND CM14.�X�֔ԍ� = CM02.�X�֔ԍ� \n"
					+    "AND CM14.�X���b�c = '" + sKey[1] + "' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetDecimal(5).ToString().Trim();
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
					sRet[7] = reader.GetString(6);
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ����}�X�^�ꗗ�擾�Q
		 * �����F����b�c�A�����
		 * �ߒl�F�X�e�[�^�X�A����b�c�A�����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_MemberTn(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "����}�X�^�ꗗ�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT ���.������ from ( "
					+ "SELECT '|' "
					+     "|| TRIM(CM01.����b�c) || '|' "
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
//					+     "|| TRIM(CM01.�����) || '|' ������ \n"
					+     "|| TRIM(CM01.�����) || '|' "
					+     "|| TRIM(�g�p�I����) || '|' "
					+     "|| TO_CHAR(SYSDATE,'YYYYMMDD') || '|' "
					+     "������ \n"
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END
					+  " FROM �b�l�O�P��� CM01\n"
					+  "     ,�b�l�O�Q���� CM02 \n"
					+  "     ,�b�l�P�S�X�֔ԍ� CM14 \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.����� LIKE '%" + sKey[1] + "%' \n";
				}
// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� START
				cmdQuery += " AND CM01.�Ǘ��ҋ敪 IN ('0','1','2') \n";
// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� END
				cmdQuery += " AND CM01.�폜�e�f = '0' \n";
				cmdQuery += " AND CM01.����b�c = CM02.����b�c \n";
				cmdQuery += " AND CM02.�폜�e�f = '0' \n";
				cmdQuery += " AND CM14.�X�֔ԍ� = CM02.�X�֔ԍ� \n";
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND CM14.�X���b�c = '" + sKey[2] + "' \n";
				}
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END
				cmdQuery += " ORDER BY CM01.����b�c \n";
				cmdQuery += " ) ��� GROUP BY ���.������ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.11.08 ���s�j�R�{ ����}�X�^��X���R�[�h�ōi�荞�� END

// ADD 2007.11.12 ���s�j���� ����}�X�^�ꗗ�̋@�\�ǉ� START
		/*********************************************************************
		 * ����}�X�^�ꗗ�擾�R
		 * �����F����b�c�A�����
		 * �ߒl�F�X�e�[�^�X�A����b�c�A�����
		 *
		 * �Q�ƌ��F��������Q.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_MemberTn3(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "����}�X�^�ꗗ�擾�R�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(CM01.����b�c) || '|' "
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
//					+     "|| TRIM(CM01.�����) || '|' ������ \n"
					+     "|| TRIM(CM01.�����) || '|' "
					+     "|| TRIM(�g�p�I����) || '|' "
					+     "|| TO_CHAR(SYSDATE,'YYYYMMDD') || '|' "
					+     "������ \n"
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END
					+     ", CM01.����b�c kcd \n"
					+  " FROM �b�l�O�P��� CM01\n";
// MOD 2007.11.14 KCL) �X�{ global�Ή� START
				cmdQuery += "     ,�b�l�O�Q���� CM02 \n";
				cmdQuery += "     ,�b�l�P�S�X�֔ԍ� CM14 \n";
// MOD 2007.11.14 KCL) �X�{ global�Ή� END
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.����� LIKE '%" + sKey[1] + "%' \n";
				}
// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� START
				cmdQuery += " AND CM01.�Ǘ��ҋ敪 IN ('0','1','2') \n";
// ADD 2010.12.14 ACT�j�_�� ���q�^���̑Ή� END
				cmdQuery += " AND CM01.�폜�e�f = '0' \n";

// MOD 2007.11.14 KCL) �X�{ global�Ή� START
				cmdQuery += " AND CM01.����b�c = CM02.����b�c \n"
					+ " AND CM02.�폜�e�f = '0' \n"
					+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+ " AND CM14.�폜�e�f = '0' \n";
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					;
				if (sKey[2].Trim().Length != 0)
					cmdQuery += " AND CM14.�X���b�c = '" + sKey[2] + "' \n";
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END
				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+ "|| TRIM(CM01.����b�c) || '|' "
					+ "|| TRIM(CM01.�����) || '|' ������ \n"
					+ ", CM01.����b�c kcd \n"
					+ " FROM �b�l�O�P��� CM01 \n"
					+ "     ,�b�l�O�T������X CM05 \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.����� LIKE '%" + sKey[1] + "%' \n";
				}
//�ۗ��F���q�Ή��́H
				cmdQuery += " AND CM01.�폜�e�f = '0' \n"
					+ " AND CM01.����b�c = CM05.����b�c \n"
					+ " AND CM05.�폜�e�f = '0' \n";
				if (sKey[2].Trim().Length != 0)
					cmdQuery += " AND CM05.�X���b�c = '" + sKey[2] + "' \n";
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 ���s�j���� �^�p���̂��q�l�̂ݑΏۋ@�\�̒ǉ� END

// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+ "|| TRIM(CM01.����b�c) || '|' "
					+ "|| TRIM(CM01.�����) || '|' ������ \n"
					+ ", CM01.����b�c kcd \n"
					+ " FROM �b�l�O�P��� CM01 \n"
					+ "     ,�b�l�O�T������X�e CM05F \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.����� LIKE '%" + sKey[1] + "%' \n";
				}
//�ۗ��F���q�Ή��́H
				cmdQuery += " AND CM01.�폜�e�f = '0' \n"
					+ " AND CM01.����b�c = CM05F.����b�c \n"
					+ " AND CM05F.�폜�e�f = '0' \n";
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND CM05F.�X���b�c = '" + sKey[2] + "' \n";
				}
				if(sKey.Length >= 4)
				{
					if(sKey[3] == "1")
					{
						cmdQuery += " AND CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END

				cmdQuery += " ORDER BY kcd \n";
// MOD 2007.11.14 KCL) �X�{ global�Ή� END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}

				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// ADD 2007.11.12 ���s�j���� ����}�X�^�ꗗ�̋@�\�ǉ� END

// ADD 2006.12.08 ���s�j�����J ���X�����擾 START
		/*********************************************************************
		 * ���X�����擾
		 * �����F���X���b�c
		 * �ߒl�F�X�e�[�^�X�A�X��������
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Hatuten(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���X���������J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �X�������� "
					+   "FROM �b�l�P�O�X�� "
					+  "WHERE �X���b�c = '" + sKey[0] + "' "
					+    "AND �폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					iCnt++;
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				if(iCnt == 1) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
					sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.08 ���s�j�����J ���X�����擾 END

// ADD 2006.12.07 ���s�j�����J ���X�d�����R�[�h�ꗗ�\�̋@�\�ǉ� START
		/*********************************************************************
		 * ���X�d�����R�[�h�ꗗ�\
		 * �����F������A���X���R�[�h
		 * �ߒl�F�X�e�[�^�X�A�X���R�[�h�A�X�����A�d�����R�[�h�A...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Tyakusiwake(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���X�d�����R�[�h�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| CM10.�X���b�c || '|' "
					+     "|| CM10.�X�������� || '|' "
					+     "|| TRIM(NVL(CM17.�d���b�c,' ')) || '|' "
					+     "|| TRIM(NVL(CM17.�d���b�c,' ')) || '|' "
					+     "|| NVL(CM17.�X�V����,'0') || '|' \n"
					+  " FROM �b�l�P�O�X�� CM10,�b�l�P�V�d�� CM17  \n"
					+ " WHERE CM10.�X���b�c = CM17.���X���b�c(+) \n"
					+   " AND '" + sKey[0] + "' = CM17.���X���b�c(+) \n"
// MOD 2015.10.09 bevas�j���{ ���X�d�����R�[�h�o�^��ʂɎ��X���Ɠ����e���g���X��\�� START
//					+   " AND CM10.�X���b�c >= '100' \n"
//					+   " AND CM10.�X���b�c <> '" + sKey[0] + "' \n"
					+   " AND CM10.�X���b�c >= '080' \n"
// MOD 2015.10.09 bevas�j���{ ���X�d�����R�[�h�o�^��ʂɎ��X���Ɠ����e���g���X��\�� END
					+   " AND CM10.�폜�e�f = '0' \n"
					+   " AND '0' = CM17.�폜�e�f(+) \n"
					+   " ORDER BY CM10.�X���b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.07 ���s�j�����J ���X�d�����R�[�h�ꗗ�\�̋@�\�ǉ� END
// ADD 2006.12.08 ���s�j�����J ���X�d�����o�^ START
		/*********************************************************************
		 * ���X�d�����ǉ�
		 * �����F���X���b�c�A���X���b�c�A�d�����b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_TyakuSiwake(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���X�d�����ǉ��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				OracleDataReader reader;
// MOD 2007.03.15 FJCS�j�K�c �o�O�C�� START
//				for(short sCnt = 0; sCnt < 17; sCnt++)
				for(short sCnt = 0; sCnt < 16; sCnt++)
// MOD 2007.03.15 FJCS�j�K�c �o�O�C�� END
				{
					if(sKey[sCnt] == null)
					{
						sCnt = 20;
					}
					else
					{
						string[] sData = sKey[sCnt].Split(',');

						cmdQuery
							= "SELECT �폜�e�f "
							+   "FROM �b�l�P�V�d�� "
							+  "WHERE ���X���b�c = '" + sData[0] + "' "
							+  "  AND ���X���b�c = '" + sData[1] + "' "
							+    "FOR UPDATE \n";

						reader = CmdSelect(sUser, conn2, cmdQuery);
						int iCnt = 1;
						string s�폜�e�f = "";
						while (reader.Read())
						{
							s�폜�e�f = reader.GetString(0);
							iCnt++;
						}
						if(iCnt == 1)
						{
							//�ǉ�
							cmdQuery
								= "INSERT INTO �b�l�P�V�d�� \n"
								+ " VALUES ('" + sData[0] + "' " 
								+         ",'" + sData[1] + "' "
								+         ",'" + sData[2] + "' "
								+         ",'0' "
								+         "," + s�X�V����
								+         ",'" + sData[3] + "' "
								+         ",'" + sData[4] + "' "
								+         "," + s�X�V����
								+         ",'" + sData[3] + "' "
								+         ",'" + sData[4] + "' "
								+ " ) \n";

							CmdUpdate(sUser, conn2, cmdQuery);

							sRet[0] = "����I��";
						}
						else
						{
							//�ǉ��X�V
							if (s�폜�e�f.Equals("1"))
							{
								cmdQuery
									= "UPDATE �b�l�P�V�d�� \n"
									+   " SET �d���b�c = '" + sData[2] + "' "
									+       ",�폜�e�f = '0' \n"
									+       ",�o�^���� = " + s�X�V����
									+       ",�o�^�o�f = '" + sData[3] + "' "
									+       ",�o�^��   = '" + sData[4] + "' "
									+       ",�X�V���� = " + s�X�V����
									+       ",�X�V�o�f = '" + sData[3] + "' "
									+       ",�X�V��   = '" + sData[4] + "' \n"
									+  "WHERE ���X���b�c = '" + sData[0] + "' "
									+  "  AND ���X���b�c = '" + sData[1] + "' \n";

								CmdUpdate(sUser, conn2, cmdQuery);

								sRet[0] = "����I��";
							}
							else
							{
								cmdQuery
									= "UPDATE �b�l�P�V�d�� \n"
									+   " SET �d���b�c = '" + sData[2] + "' "
									+       ",�폜�e�f = '0' \n"
									+       ",�X�V���� = " + s�X�V����
									+       ",�X�V�o�f = '" + sData[3] + "' "
									+       ",�X�V��   = '" + sData[4] + "' \n"
									+  "WHERE ���X���b�c = '" + sData[0] + "' "
									+  "  AND ���X���b�c = '" + sData[1] + "' \n";

								CmdUpdate(sUser, conn2, cmdQuery);

								sRet[0] = "����I��";
							}

						}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
						disposeReader(reader);
						reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
					}
				}
				tran.Commit();
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.08 ���s�j�����J ���X�d�����o�^ END
// ADD 2006.12.12 ���s�j���� ��ԏ��擾 START
		/*********************************************************************
		 * ��Ԉꗗ�擾
		 * �����F�Ȃ�
		 * �ߒl�F�X�e�[�^�X�A��Ԉꗗ
		 *********************************************************************/
		private static string GET_JYOTAI_COUNT
			= "SELECT COUNT(ROWID) \n"
			+  " FROM �`�l�O�R��� \n"
			+ " WHERE ��ԏڍׂb�c = ' ' \n"
			+ " AND �폜�e�f = '0' \n";

		private static string GET_JYOTAI
			= "SELECT ��Ԃb�c, ��Ԗ� \n"
			+  " FROM �`�l�O�R��� \n"
			+ " WHERE ��ԏڍׂb�c = ' ' \n"
			+ " AND �폜�e�f = '0' \n"
			+ " ORDER BY ��Ԃb�c, ��Ԗ� \n";

		[WebMethod]
		public string[] Get_jyotai(string[] sUser )
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "��Ԉꗗ�擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string s��Ԑ� = "0";
			int    i��Ԑ� = 0;
			try
			{
				OracleDataReader reader = CmdSelect(sUser, conn2, GET_JYOTAI_COUNT);
				if (reader.Read() )
				{
//					s��Ԑ� = reader.GetString(0);
					s��Ԑ� = reader.GetDecimal(0).ToString().Trim();
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				i��Ԑ� = int.Parse(s��Ԑ�);

				reader = CmdSelect(sUser, conn2, GET_JYOTAI);

				int iPos = 2;
				sRet = new string[i��Ԑ� * 2 + iPos];
				while (reader.Read() && iPos < sRet.Length)
				{
					sRet[iPos++] = reader.GetString(0).Trim();
					sRet[iPos++] = reader.GetString(1).Trim();
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				if(iPos > 2){
					sRet[0] = "����I��";
					sRet[1] = s��Ԑ�;
				}else{
					sRet[0] = "�T�[�o�G���[�F��ԃ}�X�^���ݒ肳��Ă��܂���";
					sRet[1] = "0";
				}

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}

			return sRet;
		}
// ADD 2006.12.12 ���s�j���� ��ԏ��擾 END
// ADD 2006.12.15 ���s�j�����J ���˗���ꗗ START
		/*********************************************************************
		 * ���˗���ꗗ�擾
		 * �����F����b�c�A�ב��l���A�ב��l�b�c�A�X���b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i���O�P�A�Z���P�A�ב��l�b�c�j...
		 *
		 * �Q�ƌ��F���˗��匟��.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Goirainusi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "���˗���ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(SM01.����b�c) || '|' "
					+     "|| TRIM(CM01.�����) || '|' "
					+     "|| TRIM(CM02.���喼) || '|' "
					+     "|| TRIM(SM01.�ב��l�b�c) || '|' "
					+     "|| TRIM(SM01.���O�P) || '|' "
					+     "|| TRIM(SM01.�Z���P) || '|' "
					+     "|| TRIM(SM01.����b�c) || '|' \n"
					+  " FROM �r�l�O�P�ב��l SM01"
					+       ",�b�l�O�Q���� CM02"
					+       ",�b�l�P�S�X�֔ԍ� CM14"
					+       ",�b�l�O�P��� CM01 \n"
					+ " WHERE SM01.����b�c =  CM01.����b�c \n";
				if (sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.�ב��l�b�c = '" + sKey[1] + "' \n";
				}
				else
				{
					if (sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.�ב��l�b�c LIKE '" + sKey[1] + "%' \n";
					}
				}
				if (sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.���O�P LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.����b�c =  CM02.����b�c \n"
					     +  " AND SM01.����b�c =  CM02.����b�c \n"
					     +  " AND CM02.�X�֔ԍ� =  CM14.�X�֔ԍ� \n"
					     +  " AND CM14.�X���b�c =  '" + sKey[3] + "' \n";
				cmdQuery += " AND SM01.�폜�e�f = '0' \n"
						 +  " AND CM02.�폜�e�f = '0' \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//						 +  " AND CM14.�폜�e�f = '0' \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
						 +  " AND CM01.�폜�e�f = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.15 ���s�j�����J ���˗���ꗗ END
// ADD 2007.11.14 KCL) �X�{ ���˗���ꗗ�Q START
		/*********************************************************************
		 * ���˗���ꗗ�擾�iglobal�Ή��j
		 * �����F����b�c�A�ב��l���A�ב��l�b�c�A�X���b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i���O�P�A�Z���P�A�ב��l�b�c�j...
		 *
		 * �Q�ƌ��F���˗��匟���Q.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Goirainusi2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "���˗���ꗗ�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(SM01.����b�c) || '|' "
					+     "|| TRIM(CM01.�����) || '|' "
					+     "|| TRIM(CM02.���喼) || '|' "
					+     "|| TRIM(SM01.�ב��l�b�c) || '|' "
					+     "|| TRIM(SM01.���O�P) || '|' "
					+     "|| TRIM(SM01.�Z���P) || '|' "
					+     "|| TRIM(SM01.����b�c) || '|' \n"
					+    ",CM01.����b�c kcd \n"
					+  " FROM �r�l�O�P�ב��l SM01"
					+       ",�b�l�O�Q���� CM02"
					+       ",�b�l�P�S�X�֔ԍ� CM14"
					+       ",�b�l�O�P��� CM01 \n"
					+ " WHERE SM01.����b�c =  CM01.����b�c \n";
				if (sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.�ב��l�b�c = '" + sKey[1] + "' \n";
				}
				else
				{
					if (sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.�ב��l�b�c LIKE '" + sKey[1] + "%' \n";
					}
				}
				if (sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.���O�P LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.����b�c =  CM02.����b�c \n"
					+  " AND SM01.����b�c =  CM02.����b�c \n"
					+  " AND CM02.�X�֔ԍ� =  CM14.�X�֔ԍ� \n"
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//					+  " AND CM14.�X���b�c =  '" + sKey[3] + "' \n";
					;
				if (sKey[3].Length != 0){
					cmdQuery += " AND CM14.�X���b�c =  '" + sKey[3] + "' \n";
				}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
				cmdQuery += " AND SM01.�폜�e�f = '0' \n"
					+  " AND CM02.�폜�e�f = '0' \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					+  " AND CM14.�폜�e�f = '0' \n"
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					+  " AND CM01.�폜�e�f = '0' \n";

				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+     "|| TRIM(SM01.����b�c) || '|' "
					+     "|| TRIM(CM01.�����) || '|' "
					+     "|| TRIM(CM02.���喼) || '|' "
					+     "|| TRIM(SM01.�ב��l�b�c) || '|' "
					+     "|| TRIM(SM01.���O�P) || '|' "
					+     "|| TRIM(SM01.�Z���P) || '|' "
					+     "|| TRIM(SM01.����b�c) || '|' \n"
					+    ",CM01.����b�c kcd \n"
					+  " FROM �r�l�O�P�ב��l SM01"
					+       ",�b�l�O�Q���� CM02"
					+       ",�b�l�O�T������X CM05"
					+       ",�b�l�O�P��� CM01 \n"
					+ " WHERE SM01.����b�c =  CM01.����b�c \n"
					+ "";
				if (sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.�ב��l�b�c = '" + sKey[1] + "' \n";
				}
				else
				{
					if (sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.�ב��l�b�c LIKE '" + sKey[1] + "%' \n";
					}
				}
				if (sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.���O�P LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.����b�c =  CM02.����b�c \n"
					+  " AND SM01.����b�c =  CM02.����b�c \n"
					+  " AND SM01.����b�c =  CM05.����b�c \n"
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//					+  " AND CM05.�X���b�c =  '" + sKey[3] + "' \n";
					;
				if (sKey[3].Length != 0){
					cmdQuery += " AND CM05.�X���b�c =  '" + sKey[3] + "' \n";
				}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
				cmdQuery += " AND SM01.�폜�e�f = '0' \n"
					+  " AND CM02.�폜�e�f = '0' \n"
					+  " AND CM05.�폜�e�f = '0' \n"
					+  " AND CM01.�폜�e�f = '0' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+     "|| TRIM(SM01.����b�c) || '|' "
					+     "|| TRIM(CM01.�����) || '|' "
					+     "|| TRIM(CM02.���喼) || '|' "
					+     "|| TRIM(SM01.�ב��l�b�c) || '|' "
					+     "|| TRIM(SM01.���O�P) || '|' "
					+     "|| TRIM(SM01.�Z���P) || '|' "
					+     "|| TRIM(SM01.����b�c) || '|' \n"
					+    ",CM01.����b�c kcd \n"
					+  " FROM �r�l�O�P�ב��l SM01"
					+       ",�b�l�O�Q���� CM02"
					+       ",�b�l�O�T������X�e CM05F"
					+       ",�b�l�O�P��� CM01 \n"
					+ " WHERE SM01.����b�c =  CM01.����b�c \n"
					+ "";
				if(sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.����b�c = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				if(sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.�ב��l�b�c = '" + sKey[1] + "' \n";
				}
				else
				{
					if(sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.�ב��l�b�c LIKE '" + sKey[1] + "%' \n";
					}
				}
				if(sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.���O�P LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.����b�c =  CM02.����b�c \n"
					+  " AND SM01.����b�c =  CM02.����b�c \n"
					+  " AND SM01.����b�c =  CM05F.����b�c \n"
					;
				if(sKey[3].Length != 0)
				{
					cmdQuery += " AND CM05F.�X���b�c =  '" + sKey[3] + "' \n";
				}
				cmdQuery += " AND SM01.�폜�e�f = '0' \n"
					+  " AND CM02.�폜�e�f = '0' \n"
					+  " AND CM05F.�폜�e�f = '0' \n"
					+  " AND CM01.�폜�e�f = '0' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END

				cmdQuery += "ORDER BY kcd \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// ADD 2007.11.14 KCL) �X�{ ���˗���ꗗ�Q END
// ADD 2006.12.18 ���s�j�����J �o�׈ꗗ�擾 START
		/*********************************************************************
		 * �o�׈ꗗ�擾
		 * �����F����b�c�A����b�c�A�ב��l�b�c�A�o�ד� or �o�^���A
		 *		 �J�n���A�I�����A��ԁA����ԍ�
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i�o�ד��A�Z���P�A���O�P�A�j...
		 *
		 *********************************************************************/
		private static string GET_SYUKKA_UNION_1 
			= "SELECT  \n"
			+       " COUNT(*), \n"
			+       " NVL(SUM(A.��),0), \n"
			+       " NVL(SUM(A.�d��),0), \n"
			+       " NVL(SUM(A.�ː�),0)  \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			+       ", NVL(SUM(A.�^���d��),0) \n"
			+       ", NVL(SUM(A.�^���ː�),0) \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
//			+       " FROM (  \n";
			+       "";
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END

		private static string GET_SYUKKA_INDEX_1 
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//			= "SELECT /*+ INDEX(S ST01IDX1) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

		private static string GET_SYUKKA_INDEX_2 
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//			= "SELECT /*+ INDEX(S ST01IDX2) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

		private static string GET_SYUKKA_INDEX_3 
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//			= "SELECT /*+ INDEX(S ST02IDX1) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

		private static string GET_SYUKKA_INDEX_4 
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//			= "SELECT /*+ INDEX(S ST02IDX2) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

		private static string GET_SYUKKA_SELECT_1 
			=       " NVL(S.��,0) AS ��, \n"
			+       " NVL(S.�d��,0) AS �d��, \n"
			+       " NVL(S.�ː�,0) AS �ː� \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			+       ", NVL(DECODE(S.�^���d��,'     ',0,S.�^���d��),0) AS �^���d�� \n"
			+       ", NVL(DECODE(S.�^���ː�,'     ',0,S.�^���ː�),0) AS �^���ː� \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
			;
//			+  " FROM \"�r�s�O�P�o�׃W���[�i��\" S, �`�l�O�R��� J \n";

		private static string GET_SYUKKA_SELECT_2 
			=       " SUBSTR(S.�o�ד�,5,2) || '/' || SUBSTR(S.�o�ד�,7,2), S.�Z���P, S.���O�P, \n"
			+       " TO_CHAR(S.��), S.�d��, S.�A���w���P, \n"
			+       " S.�i���L���P, S.�����ԍ�, DECODE(S.�����敪,1,'����',2,'����',S.�����敪), \n"
			+       " DECODE(S.�w���,0,' ',(SUBSTR(S.�w���,5,2) || '/' || SUBSTR(S.�w���,7,2) || DECODE(S.�w����敪,'0','�K��','1','�w��',''))), \n"

			+       " DECODE(S.�ڍ׏��,'  ', NVL(J.��Ԗ�, S.���),NVL(J.��ԏڍז�, S.�ڍ׏��)), \n"
			+       " SUBSTR(S.�o�^��,5,2) || '/' || SUBSTR(S.�o�^��,7,2), \n"
			+       " S.���q�l�o�הԍ�, TO_CHAR(S.\"�W���[�i���m�n\") AS �Ǘ��ԍ�, S.�o�^��, \n"
			+       " SUBSTR(S.�o�ד�,1,4) || '/' || SUBSTR(S.�o�ד�,5,2) || '/' || SUBSTR(S.�o�ד�,7,2), \n"
			+       " S.�o�^��, \n"
			+       " S.�ː�, \n"
// MOD 2007.02.20 ���s�j���� �ی����̕\�� START
//			+       " S.�ی����z, \n"
			+       " S.������, \n"
// MOD 2007.02.20 ���s�j���� �ی����̕\�� END
// MOD 2007.10.22 ���s�j���� �^���ɒ��p�������Z�\�� START
//			+       " S.�^��, \n"
			+       " S.�^�� + S.���p, \n"
// MOD 2007.10.22 ���s�j���� �^���ɒ��p�������Z�\�� END
// ADD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A����󔭍s�ςe�f��\�� START
			+       " DECODE(S.�폜�e�f,'1','��',' '), \n"
			+       " DECODE(S.����󔭍s�ςe�f,'1','��',' '), \n"
// ADD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A����󔭍s�ςe�f��\�� END
// MOD 2007.11.22 ���s�j���� �ꗗ���ڂɔ��X�b�c��\�� START
			+       " S.���X�b�c, \n"
// MOD 2007.11.22 ���s�j���� �ꗗ���ڂɔ��X�b�c��\�� END
			+       " S.�o�ד� \n"
// ADD 2008.12.01 ���s�j���� �o�׏Ɖ�̈ꗗ�̃\�[�g���̒��� START
			+       ", S.\"�W���[�i���m�n\" \n"
// ADD 2008.12.01 ���s�j���� �o�׏Ɖ�̈ꗗ�̃\�[�g���̒��� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
			+       ", S.����b�c \n"
			+       ", NVL(CM01.�����, ' ') \n"
			+       ", S.����b�c \n"
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
// MOD 2009.09.11 ���s�j���� �o�׏Ɖ�ŏo�׍ςe�f,���M�ςe�f�Ȃǂ�ǉ� START
			+       ", S.�o�׍ςe�f, S.���M�ςe�f, S.�o�^���� \n"
			+       ", S.�X�V����, S.�X�V�o�f, S.�X�V�� \n"
// MOD 2009.09.11 ���s�j���� �o�׏Ɖ�ŏo�׍ςe�f,���M�ςe�f�Ȃǂ�ǉ� END
// MOD 2010.04.06 ���s�j���� �o�׏Ɖ�ɓ��Ӑ�A�X�֔ԍ��Ȃǂ�ǉ� START
			+       ",S.���Ӑ�b�c, S.���ۂb�c, S.���ۖ� \n"
			+       ",S.�X�֔ԍ�, S.���X�b�c, S.���X�� \n"
			+       ",S.�ב��l�b�c, S.�o�^�o�f \n"
// MOD 2010.04.06 ���s�j���� �o�׏Ɖ�ɓ��Ӑ�A�X�֔ԍ��Ȃǂ�ǉ� END
// MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
			+       ", S.\"�^���G���[�m�F�e�f\" \n"
// MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2010.11.25 ���s�j���� �o�׏Ɖ�ɍ폜�����Ȃǂ�ǉ� START
			+       ", S.�폜����, S.�폜�o�f, S.�폜�� \n"
// MOD 2010.11.25 ���s�j���� �o�׏Ɖ�ɍ폜�����Ȃǂ�ǉ� END
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			+       ", S.�^���ː�, S.�^���d�� \n"
			+       ", NVL(CM01.�ۗ�����e�f,'0') \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// MOD 2014.03.19 BEVAS�j���� �z�����t�E������ǉ� START
			+       ", DECODE(S.�����O�R,'          ',' ',('20' || SUBSTR(S.�����O�R,1,2) || '/' || SUBSTR(S.�����O�R,3,2) || '/' || SUBSTR(S.�����O�R,5,2) || ' ' || SUBSTR(S.�����O�R,7,2) || ':' || SUBSTR(S.�����O�R,9,2))) \n"
// MOD 2014.03.19 BEVAS�j���� �z�����t�E������ǉ� END
			;
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
		private static string GET_SYUKKA_SELECT_22 
			=       ", S.���O�Q \n"
			+       ", S.�i���L���Q, S.�i���L���R \n"
// MOD 2011.07.28 ���s�j���� �L���s�̒ǉ� START
			+       ", S.�i���L���S, S.�i���L���T, S.�i���L���U \n"
// MOD 2011.07.28 ���s�j���� �L���s�̒ǉ� END
			;
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END

//			+ " FROM \"�r�s�O�P�o�׃W���[�i��\" S, �`�l�O�R��� J \n";

		private static string GET_SYUKKA_FROM_1 
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//			= " FROM \"�r�s�O�P�o�׃W���[�i��\" S, �`�l�O�R��� J \n";
			= " FROM \"�r�s�O�P�o�׃W���[�i��\" S \n"
			+ ", �`�l�O�R��� J \n"
			+ ", �b�l�O�P��� CM01 \n"
			;
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

		private static string GET_SYUKKA_FROM_2 
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//			= " FROM \"�r�s�O�Q�o�ח���\" S, �`�l�O�R��� J \n";
			= " FROM \"�r�s�O�Q�o�ח���\" S \n"
			+ ", �`�l�O�R��� J \n"
			+ ", �b�l�O�P��� CM01 \n"
			;
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

// MOD 2008.12.01 ���s�j���� �o�׏Ɖ�̈ꗗ�̃\�[�g���̒��� START
//		private static string GET_SYUKKA_SELECT_2_SORT
//			= " ORDER BY A.�o�^��,A.�Ǘ��ԍ� ";
//
//		private static string GET_SYUKKA_SELECT_2_SORT2
//			= " ORDER BY A.�o�ד�,A.�o�^��,A.�Ǘ��ԍ� ";
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//		private static string GET_SYUKKA_SELECT_2_SORT
//			= " ORDER BY A.�o�^��, A.\"�W���[�i���m�n\" ";
//
//		private static string GET_SYUKKA_SELECT_2_SORT2
//			= " ORDER BY A.�o�ד�, A.�o�^��, A.\"�W���[�i���m�n\" ";

		private static string GET_SYUKKA_SELECT_2_SORT
			= " ORDER BY A.����b�c, A.����b�c, A.�o�^��, A.\"�W���[�i���m�n\" ";

		private static string GET_SYUKKA_SELECT_2_SORT2
			= " ORDER BY A.�o�ד�, A.����b�c, A.����b�c, A.�o�^��, A.\"�W���[�i���m�n\" ";
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
// MOD 2008.12.01 ���s�j���� �o�׏Ɖ�̈ꗗ�̃\�[�g���̒��� END

		[WebMethod]
		public String[] Get_syukka(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�o�׈ꗗ�擾�J�n");

			OracleConnection conn2 = null;
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
//			string[] sRet = new string[4];
			string[] sRet = new string[10]{"", "0", "0", "0", ""
											,"" ,"" ,"" ,"" ,""};
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string s�o�^���� = "0";
			string s�����v = "0";
			int    i�o�^���� = 0;
			decimal d�d�ʍ��v = 0;
			decimal d�ː����v = 0;
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			decimal d�^���d�ʌv = 0;
			decimal d�^���ː��v = 0;
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
			string s�����    = "";
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
			string s����b�c = "";
			string s����b�c = "";
			string s�ב��l�b�c = "";
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
			bool   b���o�� = sKey[6].Equals("90");
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
//�ۗ�			bool   b�^���G���[ = sKey[6].Equals("91");
//�ۗ�			if(b�^���G���[) sKey[8] = "1";
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
			string  s�^���ː� = "";
			string  s�^���d�� = "";
			decimal d�ː��d�� = 0;
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
			string  s����������� = (sKey.Length > 10) ? sKey[10] : "0";
			bool    b����������� = (s����������� == "1") ? true : false;
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			StringBuilder sbRet = new StringBuilder(1024);
			try
			{
				if(sKey[7].Length == 0)
				{
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//					sbQuery.Append(" WHERE S.����b�c = '" + sKey[0] + "' \n");
					if(b���o��){
						sbQuery.Append(", �r�s�O�T���o�ו� ST05 \n");
						if(sKey[0].Length > 0){
							sbQuery.Append(" WHERE ST05.����b�c = '" + sKey[0] + "' \n");
						}else{
							sbQuery.Append(" WHERE ST05.����b�c > ' ' \n");
						}
						if(sKey[1].Length > 0)
						{
							sbQuery.Append(" AND ST05.����b�c = '" + sKey[1] + "' \n");
						}
						if(sKey[3] == "0")
						{
							sbQuery.Append(" AND ST05.�o�ד�  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
						}
						else
						{
							sbQuery.Append(" AND ST05.�o�^��  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
						}
						sbQuery.Append(" AND ST05.�o�ד� < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
						sbQuery.Append(" AND ST05.���   = '02' \n");
						if(sKey[9].Length > 0){
							sbQuery.Append(" AND ST05.���X�b�c = '"+ sKey[9] + "' \n");
						}
						sbQuery.Append(" AND ST05.����b�c = S.����b�c \n");
						sbQuery.Append(" AND ST05.����b�c = S.����b�c \n");
						sbQuery.Append(" AND ST05.�o�^�� = S.�o�^�� \n");
						sbQuery.Append(" AND ST05.\"�W���[�i���m�n\" = S.\"�W���[�i���m�n\" \n");
					}else{
						if(sKey[0].Length > 0){
							sbQuery.Append(" WHERE S.����b�c = '" + sKey[0] + "' \n");
						}else{
							sbQuery.Append(" WHERE S.����b�c > ' ' \n");
						}
						if(sKey[1].Length > 0)
						{
							sbQuery.Append("   AND S.����b�c = '" + sKey[1] + "' \n");
						}
					}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

					if(sKey[2].Length > 0)
					{
						sbQuery.Append(" AND S.�ב��l�b�c = '"+ sKey[2] + "' \n");
					}

					if(sKey[3] == "0")
					{
						sbQuery.Append(" AND S.�o�ד�  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND S.�o�^��  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
					}
					
					if(sKey[6] != "00")
					{
						if(sKey[6] == "aa")
							sbQuery.Append(" AND S.��� <> '01' \n");
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
						else if(b���o��){
							sbQuery.Append(" AND S.�o�ד� < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
							sbQuery.Append(" AND S.���   = '02' \n");
						}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
//�ۗ�						else if(b�^���G���[){
//�ۗ�							sbQuery.Append(" AND ( S.�^�� > 0 OR S.���p > 0 OR S.������ > 0 ) \n");
//�ۗ�						}
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
						else
							sbQuery.Append(" AND S.��� = '"+ sKey[6] + "' \n");
					}
				}
				else
				{
					sbQuery.Append(" WHERE S.�����ԍ� = '"+ sKey[7] + "' \n");
				}
				if(sKey[8] != "0")
				{
					if(sKey[8] == "1")
					{
// MOD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A���x������ϋ敪��\�� START
//						sbQuery.Append(" AND S.�폜�e�f <> '0' \n");
						sbQuery.Append(" AND S.�폜�e�f = '1' \n");
// MOD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A���x������ϋ敪��\�� END
					}
					else
					{
						sbQuery.Append(" AND S.�폜�e�f = '0' \n");
					}
				}

// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
				if(sKey[9].Length > 0){
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� START
					sbQuery.Append(" AND S.���X�b�c = '"+ sKey[9] + "' \n");
//					sbQuery.Append(" AND ( S.���X�b�c = '"+ sKey[9] + "' OR S.�o�^�� = '"+ sKey[9] + "') \n");
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
				}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
				sbQuery.Append(" AND S.���     = J.��Ԃb�c(+) \n");
				sbQuery.Append(" AND S.�ڍ׏�� = J.��ԏڍׂb�c(+) \n");
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
				sbQuery.Append(" AND S.����b�c = CM01.����b�c(+) \n");
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END

				sbQuery2.Append(GET_SYUKKA_UNION_1);
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
				//�����ԍ������͂���Ă���ꍇ
				if(sKey[7].Length > 0)
				{
					sbQuery2.Append(", NVL(MIN(A.����b�c),' ') \n");
					sbQuery2.Append(", NVL(MIN(A.����b�c),' ') \n");
					sbQuery2.Append(", NVL(MIN(A.�ב��l�b�c),' ') \n");
				}
				sbQuery2.Append(" FROM (  \n");
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
				if(sKey[7].Length == 0)
				{
					sbQuery2.Append(GET_SYUKKA_INDEX_2);
				}
				else
				{
					sbQuery2.Append(GET_SYUKKA_INDEX_1);
				}
				sbQuery2.Append(GET_SYUKKA_SELECT_1);
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
				//�����ԍ������͂���Ă���ꍇ
				if(sKey[7].Length > 0)
				{
					sbQuery2.Append(", NVL(S.����b�c, ' ')   AS ����b�c \n");
					sbQuery2.Append(", NVL(S.����b�c, ' ')   AS ����b�c \n");
					sbQuery2.Append(", NVL(S.�ב��l�b�c, ' ') AS �ב��l�b�c \n");
				}
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
				sbQuery2.Append(GET_SYUKKA_FROM_1);
				sbQuery2.Append(sbQuery);
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
				if(!b���o��){
//�ۗ�				if(!b���o�� && !b�^���G���[){
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
					sbQuery2.Append(" UNION ALL \n");
					if(sKey[7].Length == 0)
					{
						sbQuery2.Append(GET_SYUKKA_INDEX_4);
					}
					else
					{
						sbQuery2.Append(GET_SYUKKA_INDEX_3);
					}
					sbQuery2.Append(GET_SYUKKA_SELECT_1);
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
					//�����ԍ������͂���Ă���ꍇ
					if(sKey[7].Length > 0)
					{
						sbQuery2.Append(", NVL(S.����b�c, ' ')   AS ����b�c \n");
						sbQuery2.Append(", NVL(S.����b�c, ' ')   AS ����b�c \n");
						sbQuery2.Append(", NVL(S.�ב��l�b�c, ' ') AS �ב��l�b�c \n");
					}
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
					sbQuery2.Append(GET_SYUKKA_FROM_2);
					sbQuery2.Append(sbQuery);
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
				}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
				sbQuery2.Append(" ) A \n");

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery2);

				if(reader.Read())
				{
					s�o�^����   = reader.GetDecimal(0).ToString("#,##0").Trim();
					s�����v   = reader.GetDecimal(1).ToString("#,##0").Trim();
					d�d�ʍ��v   = reader.GetDecimal(2);
					d�ː����v   = reader.GetDecimal(3);
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
					//�����ԍ������͂���Ă���ꍇ
					if(sKey[7].Length > 0)
					{
// MOD 2011.05.18 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
//						s����b�c   = reader.GetString(4).Trim();
//						s����b�c   = reader.GetString(5).Trim();
//						s�ב��l�b�c = reader.GetString(6).Trim();
//// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
//						d�^���d�ʌv   = reader.GetDecimal(7);
//						d�^���ː��v   = reader.GetDecimal(8);
						d�^���d�ʌv   = reader.GetDecimal(4);
						d�^���ː��v   = reader.GetDecimal(5);
						s����b�c   = reader.GetString(6).Trim();
						s����b�c   = reader.GetString(7).Trim();
						s�ב��l�b�c = reader.GetString(8).Trim();
// MOD 2011.05.18 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
					}else{
						d�^���d�ʌv   = reader.GetDecimal(4);
						d�^���ː��v   = reader.GetDecimal(5);
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
					}
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet[1] = s�o�^����;
				sRet[2] = s�����v;
				d�d�ʍ��v = d�d�ʍ��v + d�ː����v * 8;
				sRet[3] = d�d�ʍ��v.ToString("#,##0").Trim();
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
				if(sUser.Length > 3 && sUser[3].Length >= 4){ // 2.10�ȍ~�i�Վ��Ή��j
					d�^���d�ʌv = d�^���d�ʌv + d�^���ː��v * 8;
					sRet[3] += "|" + d�^���d�ʌv.ToString("#,##0").Trim();
				}
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
				sRet[4] = s����b�c;
				sRet[5] = s����b�c;
				sRet[6] = s�ב��l�b�c;
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END

				i�o�^���� = int.Parse(s�o�^����.Replace(",",""));

				if(i�o�^���� == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
// MOD 2007.01.23 ���s�j���� 1000���ȏ�͕\���ł��Ȃ����� START
//				else if(i�o�^���� > 5000)
//				{
//					sRet[0] = "5000���I�[�o�[";
//					logWriter(sUser, INF, sRet[0]);
//					return sRet;
//				}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
//				else if(i�o�^���� > 1000)
				else if(i�o�^���� > 1000 && b����������� == false)
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
				{
					sRet[0] = "1000���I�[�o�[";
					logWriter(sUser, INF, sRet[0]);
					return sRet;
				}
// MOD 2007.01.23 ���s�j���� 1000���ȏ�͕\���ł��Ȃ����� END
				else
				{
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
//					sRet = new string[i�o�^���� + 4];
					sRet = new string[i�o�^���� + 10];
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
					sRet[0] = "����I��";
					sRet[1] = s�o�^����;
					sRet[2] = s�����v;
					sRet[3] = d�d�ʍ��v.ToString("#,##0").Trim();
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
					if(sUser.Length > 3 && sUser[3].Length >= 4){ // 2.10�ȍ~�i�Վ��Ή��j
						sRet[3] += "|" + d�^���d�ʌv.ToString("#,##0").Trim();
					}
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
					sRet[4] = s����b�c;
					sRet[5] = s����b�c;
					sRet[6] = s�ב��l�b�c;
					string[] sIRet = Get_Sirainusi(sUser, s����b�c, s����b�c, s�ב��l�b�c, sKey[9]);
					if(sIRet[0].Length == 4){
						sRet[7] = sIRet[1];
						sRet[8] = sIRet[2];
						sRet[9] = sIRet[3];
					}
// ADD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END

					sbQuery2 = new StringBuilder(1024);
					if(sKey[3] == "0")
					{
						sbQuery2.Append(" SELECT * FROM ( \n");
						if(sKey[7].Length == 0)
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_2);
						}
						else
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_1);
						}
						sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
						if(b�����������){
							sbQuery2.Append(GET_SYUKKA_SELECT_22);
						}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
						sbQuery2.Append(GET_SYUKKA_FROM_1);
						sbQuery2.Append(sbQuery);
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
						if(!b���o��){
//�ۗ�						if(!b���o�� && !b�^���G���[){
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
							sbQuery2.Append(" UNION ALL \n");
							if(sKey[7].Length == 0)
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_4);
							}
							else
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_3);
							}
							sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
							if(b�����������){
								sbQuery2.Append(GET_SYUKKA_SELECT_22);
							}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
							sbQuery2.Append(GET_SYUKKA_FROM_2);
							sbQuery2.Append(sbQuery);
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
						}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
						sbQuery2.Append(" ) A \n");
						sbQuery2.Append(GET_SYUKKA_SELECT_2_SORT2);
						reader = CmdSelect(sUser, conn2, sbQuery2);
					}
					else
					{
						sbQuery2.Append(" SELECT * FROM ( \n");
						if(sKey[7].Length == 0)
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_2);
						}
						else
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_1);
						}
						sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
						if(b�����������){
							sbQuery2.Append(GET_SYUKKA_SELECT_22);
						}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
						sbQuery2.Append(GET_SYUKKA_FROM_1);
						sbQuery2.Append(sbQuery);
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
						if(!b���o��){
//�ۗ�						if(!b���o�� && !b�^���G���[){
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
							sbQuery2.Append(" UNION ALL \n");
							if(sKey[7].Length == 0)
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_4);
							}
							else
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_3);
							}
							sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
							if(b�����������){
								sbQuery2.Append(GET_SYUKKA_SELECT_22);
							}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
							sbQuery2.Append(GET_SYUKKA_FROM_2);
							sbQuery2.Append(sbQuery);
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
						}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
						sbQuery2.Append(" ) A \n");
						sbQuery2.Append(GET_SYUKKA_SELECT_2_SORT);
						reader = CmdSelect(sUser, conn2, sbQuery2);
					}

// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� START
//					int iCnt = 4;
					int iCnt = 10;
// MOD 2007.01.11 ���s�j���� �����ԍ��Ō����������A���q�l���A�˗��喼��\�� END
					while (reader.Read() && iCnt < sRet.Length)
					{
						sbRet = new StringBuilder(1024);

// ADD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A����󔭍s�ςe�f��\�� START
						sbRet.Append(sSepa + reader.GetString(20));			// �폜�e�f
						sbRet.Append(sSepa + reader.GetString(21));			// ����󔭍s�ςe�f
// ADD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A����󔭍s�ςe�f��\�� END
						sbRet.Append(sSepa + reader.GetString(0));			// �o�ד�
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
						if(sKey[7].Length == 0 && b���o��){
//�ۗ�						if(sKey[7].Length == 0 && ( b���o�� || b�^���G���[ )){
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
							sbRet.Append(sSepa + reader.GetString(25).Trim());	// ����b�c
							sbRet.Append(sCRLF + reader.GetString(26).Trim());	// �����
						}else{
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
							sbRet.Append(sSepa + reader.GetString(1).Trim());	// �Z���P
							sbRet.Append(sCRLF + reader.GetString(2).Trim());	// ���O�P
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
							if(b�����������){
// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ� START
//								sbRet.Append(sCRLF + reader.GetString(49).Trim()); // ���O�Q
								sbRet.Append(sCRLF + reader.GetString(50).Trim()); // ���O�Q
// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ� END
							}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
// MOD 2009.05.11 ���s�j���� ���o�בΉ� START
						}
// MOD 2009.05.11 ���s�j���� ���o�בΉ� END
						sbRet.Append(sSepa + reader.GetString(3));			// ��
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
//						d�ː����v = reader.GetDecimal(17);
//						d�ː����v = d�ː����v * 8;
//						if(d�ː����v == 0)
//							sbRet.Append(sSepa + reader.GetDecimal(4).ToString("#,##0").Trim()); // �d��
//						else
//							sbRet.Append(sSepa + d�ː����v.ToString("#,##0").Trim());		// �ː�
						// ���q�l���͒l
						d�ː����v = reader.GetDecimal(17) * 8;
						d�ː����v += reader.GetDecimal(4);
						if(d�ː����v == 0){
//							sbRet.Append(sSepa + " ");
							sbRet.Append(sSepa + "0");
						}else{
							sbRet.Append(sSepa + d�ː����v.ToString("#,##0").Trim());
						}
						if(sUser.Length > 3 && sUser[3].Length >= 4){ // 2.10�ȍ~�i�Վ��Ή��j
							s�^���ː� = reader.GetString(46).TrimEnd();
							s�^���d�� = reader.GetString(47).TrimEnd();
							d�ː��d�� = 0;
							if(s�^���ː�.Length > 0){
								try{
									d�ː��d�� += (Decimal.Parse(s�^���ː�) * 8);
								}catch(Exception){}
							}
							if(s�^���d��.Length > 0){
								try{
									d�ː��d�� += Decimal.Parse(s�^���d��);
								}catch(Exception){}
							}
							if(d�ː��d�� == 0){
								sbRet.Append(sSepa + " ");
							}else{
								sbRet.Append(sSepa + d�ː��d��.ToString("#,##0").Trim());
							}
						}
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
// MOD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A����󔭍s�ςe�f��\�� START
//						sbRet.Append(sSepa + reader.GetDecimal(18).ToString("#,##0").Trim());
//																			// �ی���
//						sbRet.Append(sSepa + reader.GetDecimal(19).ToString("#,##0").Trim());
//																			// �^��
//
//						sbRet.Append(sSepa + reader.GetString(5).TrimEnd());		// �A���w���P
//						sbRet.Append(sCRLF + reader.GetString(6).Trim());		// �i���L���P
//						s����� = reader.GetString(7).Trim();              		// �����ԍ�
//						if(s�����.Length == 0)
//							sbRet.Append(sSepa + s�����);
//						else
//							sbRet.Append(sSepa + s�����.Remove(0,4));
//						sbRet.Append(sCRLF + reader.GetString(8));			// �����敪
//						sbRet.Append(sSepa + reader.GetString(9));			// �w���
//						sbRet.Append(sSepa + reader.GetString(10).Trim());	// ���
//						sbRet.Append(sSepa + reader.GetString(11));			// �o�^��
//						sbRet.Append(sSepa + reader.GetString(12).Trim());	// ���q�l�o�הԍ�
						s����� = reader.GetString(7).Trim();              		// �����ԍ�
						if(s�����.Length == 0)
							sbRet.Append(sSepa + s�����);
						else
							sbRet.Append(sSepa + s�����.Remove(0,4));
						sbRet.Append(sCRLF + reader.GetString(8));			// �����敪
// ADD 2007.11.22 ���s�j���� �ꗗ���ڂɔ��X�b�c��\�� START
						sbRet.Append("�@" + reader.GetString(22));			// ���X�b�c
// ADD 2007.11.22 ���s�j���� �ꗗ���ڂɔ��X�b�c��\�� END
						sbRet.Append(sSepa + reader.GetString(12).Trim());	// ���q�l�o�הԍ�
						sbRet.Append(sSepa + reader.GetString(9));			// �w���
						sbRet.Append(sSepa + reader.GetString(10).Trim());	// ���
// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ� START
						sbRet.Append(sSepa + reader.GetString(49).Trim());	// �z�����t�E����
// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ� END
						sbRet.Append(sSepa + reader.GetString(5).TrimEnd());// �A���w���P
						sbRet.Append(sCRLF + reader.GetString(6).Trim());	// �i���L���P
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� START
						if(b�����������){
// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ� START
//							sbRet.Append(sCRLF + reader.GetString(50).Trim()); // �i���L���Q
//							sbRet.Append(sCRLF + reader.GetString(51).Trim()); // �i���L���R
//							sbRet.Append(sCRLF + reader.GetString(52).Trim()); // �i���L���S
//							sbRet.Append(sCRLF + reader.GetString(53).Trim()); // �i���L���T
//							sbRet.Append(sCRLF + reader.GetString(54).Trim()); // �i���L���U
							sbRet.Append(sCRLF + reader.GetString(51).Trim()); // �i���L���Q
							sbRet.Append(sCRLF + reader.GetString(52).Trim()); // �i���L���R
							sbRet.Append(sCRLF + reader.GetString(53).Trim()); // �i���L���S
							sbRet.Append(sCRLF + reader.GetString(54).Trim()); // �i���L���T
							sbRet.Append(sCRLF + reader.GetString(55).Trim()); // �i���L���U
// MOD 2014.03.19 BEVAS�j���� �o�׏Ɖ�ɔz�����t�E������ǉ� END
// MOD 2011.07.28 ���s�j���� �L���s�̒ǉ� END
						}
// MOD 2011.05.20 ���s�j���� �o�׏Ɖ���̌���������� END
						sbRet.Append(sSepa + reader.GetDecimal(19).ToString("#,##0").Trim());
																			// �^��
						sbRet.Append(sSepa + reader.GetDecimal(18).ToString("#,##0").Trim());
																			// �ی���
						sbRet.Append(sSepa + reader.GetString(11));			// �o�^��
// MOD 2007.01.17 ���s�j���� �ꗗ���ڂɍ폜�e�f�A����󔭍s�ςe�f��\�� END
						sbRet.Append(sSepa + reader.GetString(13));			// �W���[�i���m�n
						sbRet.Append(sSepa + reader.GetString(14));			// �o�^��
						sbRet.Append(sSepa + reader.GetString(15));			// �o�ד�
						sbRet.Append(sSepa + reader.GetString(16));			// �o�^��
// MOD 2009.09.11 ���s�j���� �o�׏Ɖ�ŏo�׍ςe�f,���M�ςe�f�Ȃǂ�ǉ� START
						sbRet.Append(sSepa + reader.GetString(28));			// �o�׍ςe�f
						sbRet.Append(sSepa + reader.GetString(29));			// ���M�ςe�f
						sbRet.Append(sSepa + reader.GetDecimal(30).ToString());
																			// �o�^����
						sbRet.Append(sSepa + reader.GetDecimal(31).ToString());
																			// �X�V����
						sbRet.Append(sSepa + reader.GetString(32));			// �X�V�o�f
						sbRet.Append(sSepa + reader.GetString(33));			// �X�V��
// MOD 2009.09.11 ���s�j���� �o�׏Ɖ�ŏo�׍ςe�f,���M�ςe�f�Ȃǂ�ǉ� END
// MOD 2010.04.06 ���s�j���� �o�׏Ɖ�ɓ��Ӑ�A�X�֔ԍ��Ȃǂ�ǉ� START
						sbRet.Append(sSepa + reader.GetString(34).TrimEnd()); // ���Ӑ�b�c
						sbRet.Append(sSepa + reader.GetString(35).TrimEnd()); // ���ۂb�c
						sbRet.Append(sSepa + reader.GetString(36).TrimEnd()); // ���ۖ�

						sbRet.Append(sSepa + reader.GetString(37).TrimEnd()); // �X�֔ԍ�
						sbRet.Append(sSepa + reader.GetString(38).TrimEnd()); // ���X�b�c
						sbRet.Append(sSepa + reader.GetString(39).TrimEnd()); // ���X��

						sbRet.Append(sSepa + reader.GetString(25).TrimEnd()); // ����b�c
						sbRet.Append(sSepa + reader.GetString(27).TrimEnd()); // ����b�c
						sbRet.Append(sSepa + reader.GetString(40).TrimEnd()); // �ב��l�b�c
// MOD 2010.04.06 ���s�j���� �o�׏Ɖ�ɓ��Ӑ�A�X�֔ԍ��Ȃǂ�ǉ� END
// MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
						sbRet.Append(sSepa + reader.GetString(42).TrimEnd()); // �^���G���[�m�F�e�f
// MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2010.11.25 ���s�j���� �o�׏Ɖ�ɍ폜�����Ȃǂ�ǉ� START
						sbRet.Append(sSepa + reader.GetDecimal(43).ToString());
																			// �폜����
						sbRet.Append(sSepa + reader.GetString(44));			// �폜�o�f
						sbRet.Append(sSepa + reader.GetString(45));			// �폜��
// MOD 2010.11.25 ���s�j���� �o�׏Ɖ�ɍ폜�����Ȃǂ�ǉ� END
						sbRet.Append(sSepa);
						sRet[iCnt] = sbRet.ToString();
						iCnt++;
					}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
				}

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}
// ADD 2006.12.18 ���s�j�����J �o�׈ꗗ�擾 END
// ADD 2006.12.20 ���s�j���� �ғ��������\�̋@�\�ǉ� START
		/*********************************************************************
		 * �ғ��������\�b�r�u�o�͗p�Q
		 * �����F������A�n��i�J�n�A�I���j�A�X���i�J�n�A�I���j�A
		 *		 ���t�i�J�n�A�I���j
		 * �ߒl�F�X�e�[�^�X�A�X�����A���Ӑ敔�ۖ��A...
		 *
		 *********************************************************************/
		private static string GET_KADOURITU_CSV2_SELECT1
			= ""
			+ "SELECT * FROM ( \n"
			+ " SELECT \n"
			+ "  MAX(�n��P) AS �n��P \n"
			+ ", MAX(�n��Q) AS �n��Q \n"
			+ ", �X���b�c \n"
			+ ", MAX(�X����)   AS �X���� \n"
// MOD 2007.08.29 ���s�j���� �ғ����W�v�̕ύX START
//			+ ", MAX(�����䐔) AS �����䐔 \n"
//			+ ", MAX(�ғ��䐔) AS �ғ��䐔 \n"
			+ ", SUM(�����䐔) AS �����䐔 \n"
			+ ", SUM(�ғ��䐔) AS �ғ��䐔 \n"
// MOD 2007.08.29 ���s�j���� �ғ����W�v�̕ύX END
			+ " FROM �r�s�O�R�ғ��� \n"
			;
// ADD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ� START
		private static string GET_KADOURITU_CSV2_SELECT1_2
			= ""
			+ "SELECT * FROM ( \n"
			+ " SELECT \n"
			+ "  MAX(�n��P) AS �n��P \n"
			+ ", MAX(�n��Q) AS �n��Q \n"
			+ ", �X���b�c \n"
			+ ", MAX(�X����)   AS �X���� \n"
			+ ", SUM(DECODE(�W�v�敪,'D',�ғ��䐔,�����䐔)) AS �����䐔 \n"
			+ ", SUM(�ғ��䐔) AS �ғ��䐔 \n"
			+ " FROM �r�s�O�R�ғ��� \n"
			;
// ADD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ� END
		private static string GET_KADOURITU_CSV2_SELECT2
			= ""
			+ " GROUP BY �X���b�c \n"
			+ " ) \n"
			+ " WHERE ( �����䐔 > 0 OR �ғ��䐔 > 0 OR �n��P > ' ' OR �n��Q > ' ' ) \n"
			;
		private static string GET_KADOURITU_CSV2_ORDER
			= ""
			+ " ORDER BY �n��P, �n��Q, �X���b�c \n"
			;

		[WebMethod]
		public String[] Get_kadouritu_csv2(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�ғ��������\�b�r�u�o�͗p�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
// MOD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ� START
//				sbQuery.Append(GET_KADOURITU_CSV2_SELECT1);
				if(sData.Length >= 7 && sData[6] == "1"){
					sbQuery.Append(GET_KADOURITU_CSV2_SELECT1_2);
				}else{
					sbQuery.Append(GET_KADOURITU_CSV2_SELECT1);
				}
// MOD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ� END
				sbQuery.Append("WHERE �o�ד��J�n = '"+ sData[4] + "' \n");
// MOD 2007.02.01 ���s�j���� �ғ��������\�̏����ύX START
//				sbQuery.Append("  AND �o�ד��I�� = '"+ sData[5] + "' \n");
				if(sData[5] != null && sData[5].Trim().Length > 0){
					sbQuery.Append("  AND �o�ד��I�� = '"+ sData[5] + "' \n");
				}
// MOD 2007.02.01 ���s�j���� �ғ��������\�̏����ύX END
// MOD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ� START
				if(sData.Length >= 7 && sData[6] == "1"){
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
					//���q�^�����̓����֘A�W�v�������ɒǉ��i�W�v�敪�F�uF�v�j
//					sbQuery.Append("  AND �W�v�敪 <= 'D' \n");
					sbQuery.Append("  AND �W�v�敪 in ('A', 'B', 'C', 'D', 'F') \n");
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
				}
// MOD 2007.10.11 ���s�j���� �f���@�������@�\�̒ǉ� END

// DEL 2007.02.01 ���s�j���� �ғ��������\�̏����ύX START
//				//�n��
//				if(sData[0].Length > 0)
//				{
//					if(sData[1].Length > 0)
//					{
//						sbQuery.Append(" AND �n��P  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
//					}
//					else
//					{
//						sbQuery.Append(" AND �n��P = '"+ sData[0] + "' \n");
//					}
//				}
// DEL 2007.02.01 ���s�j���� �ғ��������\�̏����ύX START

				//�X��
				if(sData[2].Length > 0)
				{
					if(sData[3].Length > 0)
					{
						sbQuery.Append(" AND �X���b�c  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND �X���b�c = '"+ sData[2] + "' \n");
					}
				}

				sbQuery.Append(GET_KADOURITU_CSV2_SELECT2);

// ADD 2007.02.01 ���s�j���� �ғ��������\�̏����ύX START
				//�n��
				if(sData[0].Length > 0)
				{
					if(sData[1].Length > 0)
					{
						sbQuery.Append(" AND �n��P  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND �n��P = '"+ sData[0] + "' \n");
					}
				}
// ADD 2007.02.01 ���s�j���� �ғ��������\�̏����ύX START

				sbQuery.Append(GET_KADOURITU_CSV2_ORDER);

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);;

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read())
				{
					sbData = new StringBuilder(1024);
					sbData.Append(         sDbl + sSng + reader.GetString(0).Trim() + sDbl);	// �n��P
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(1).Trim() + sDbl);	// �n��Q
//�ۗ��F�s�v�ȍ��ڂ͍폜�ł��邩�H
					sbData.Append(sKanma + sDbl + sDbl);										// �n��P����
					sbData.Append(sKanma + sDbl + sDbl);										// �n��Q����
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(2).Trim() + sDbl);	// �X���b�c
					sbData.Append(sKanma + sDbl        + reader.GetString(3).Trim() + sDbl);	// �X����
					double d�����䐔 = double.Parse(reader.GetDecimal(4).ToString().Trim());	// �����䐔
					double d�ғ��䐔 = double.Parse(reader.GetDecimal(5).ToString().Trim());	// �ғ��䐔
					sbData.Append(sKanma + sDbl        + d�����䐔 + sDbl);
					sbData.Append(sKanma + sDbl        + d�ғ��䐔 + sDbl);
//�ۗ��F�v�Z�͂d�w�b�d�k�Ɉړ��ł��邩�H
					if(d�����䐔 == 0 || d�ғ��䐔 == 0){
						sbData.Append(sKanma + sDbl + "0.0" + sDbl);
					}else{
						sbData.Append(sKanma + sDbl + (d�ғ��䐔 * 100 / d�����䐔).ToString("00.0") + sDbl);
					}

					sList.Add(sbData);
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.20 ���s�j���� �ғ��������\�̋@�\�ǉ� END
// ADD 2006.12.20 ���s�j���� �o�׏󋵈ꗗ�\�̋@�\�ǉ� START
		/*********************************************************************
		 * �o�׏󋵈ꗗ�\�i�b�r�u�o�͗p�j�Q
		 * �����F������A�n��i�J�n�A�I���j�A�X���i�J�n�A�I���j�A
		 *		 �o�ד��i�J�n�A�I���j
		 * �ߒl�F�X�e�[�^�X�A�X�����A���Ӑ敔�ۖ��A...
		 *
		 *********************************************************************/
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//		private static string GET_SYUUKA_INF2_SELECT
//			= "SELECT NVL(CM10.�n��P,' ') AS �n��P \n"
//			+      ", ST04.�X���b�c \n"
//			+      ", NVL(CM10.�X����, ST04.�X���b�c) AS �X���� \n"
//			+      ", ST04.�ב��l�b�c \n"
//			+      ", SM01.���O�P \n"
//			+      ", SM01.���O�Q \n"
//			+      ", ST04.���Ӑ�b�c  \n"
//			+      ", ST04.���ۂb�c \n"
//			+      ", ST04.���ۖ� \n"
//			+      ", ST04.�o�ד� \n"
//			+      ", ST04.���� \n"
//			+      ", ST04.�� \n"
//			+      ", ST04.�d�� + (ST04.�ː� * 8) AS �d�� \n"
////�ۗ��F���g�p�H
////			+      ", ST04.���ʏd�� + (ST04.���ʍː� * 8) AS ���ʏd�� \n"
//			+      ", 0 AS ���ʏd�� \n"
//			+      ", ST04.�^�� AS �^�� \n"
//			+      ", ST04.���p AS ���p�� \n"
//			+      ", ST04.�ی����z  AS �ی��� \n"
//			+      " FROM \n"
//			+      "  �r�s�O�S�o�׏� ST04 \n"
//			+      ", �b�l�P�O�X��     CM10 \n"
//			+      ", �r�l�O�P�ב��l   SM01 \n"
//			;
		private static string GET_SYUUKA_INF2_SELECT
			= "SELECT NVL(CM10.�n��P,' ') AS �n��P \n"
			+      ", NVL(CM10.�n��Q,' ') AS �n��Q \n"
			+      ", ST04W.�X���b�c \n"
			+      ", NVL(CM10.�X����,' ') \n"
			+      ", ST04W.����b�c \n"
			+      ", NVL(CM01.�����,' ') \n"
			+      ", ST04W.����b�c \n"
			+      ", NVL(CM02.���喼,' ') \n"
			+      ", ST04W.���� \n"
			+      ", ST04W.�� \n"
			+      ", ST04W.�d�� + (ST04W.�ː� * 8) \n"
			+      ", ST04W.�^�� \n"
			+      ", ST04W.���p \n"
			+      ", ST04W.�ی����z \n"
			+      " FROM \n"
			+      "(SELECT \n"
			+      "  ST04.�X���b�c \n"
			+      ", ST04.����b�c \n"
			+      ", ST04.����b�c \n"
			+      ", SUM(ST04.����)     AS ���� \n"
			+      ", SUM(ST04.��)     AS �� \n"
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
//			+      ", SUM(ST04.�d��)     AS �d�� \n"
//			+      ", SUM(ST04.�ː�)     AS �ː� \n"
			+      ", SUM(ST04.���ʏd��) AS �d�� \n"
			+      ", SUM(ST04.���ʍː�) AS �ː� \n"
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
			+      ", SUM(ST04.�^��)     AS �^�� \n"
			+      ", SUM(ST04.���p)     AS ���p \n"
			+      ", SUM(ST04.�ی����z) AS �ی����z \n"
			+      "  FROM �r�s�O�S�o�׏� ST04 \n"
			;

		private static string GET_SYUUKA_INF2_FROM
			=      ") ST04W \n"
			+      ", �b�l�P�O�X��     CM10 \n"
			+      ", �b�l�O�P���     CM01 \n"
			+      ", �b�l�O�Q����     CM02 \n"
			;
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

		[WebMethod]
		public String[] Get_syuuka_Inf2(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�o�׏󋵈ꗗ�\�o�͗p�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			StringBuilder sbQuery  = new StringBuilder(1024);
			try
			{
				sbQuery.Append(GET_SYUUKA_INF2_SELECT);
				sbQuery.Append(" WHERE ST04.�o�ד� BETWEEN '"+ sData[4] + "' AND '"+ sData[5] +"' \n");

// ADD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
				//�X��
				if(sData[2].Length > 0)
				{
					if(sData[3].Length > 0)
					{
						sbQuery.Append(" AND ST04.�X���b�c  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND ST04.�X���b�c = '"+ sData[2] + "' \n");
					}
				}
				sbQuery.Append(" GROUP BY ST04.�X���b�c, ST04.����b�c, ST04.����b�c \n");
				sbQuery.Append(GET_SYUUKA_INF2_FROM);
// ADD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

				//�n��
				if(sData[0].Length > 0)
				{
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//					sbQuery.Append(" AND ST04.�X���b�c = CM10.�X���b�c \n");
					sbQuery.Append(" WHERE ST04W.�X���b�c = CM10.�X���b�c \n");
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END
					if(sData[1].Length > 0)
					{
						sbQuery.Append(" AND CM10.�n��P BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND CM10.�n��P = '"+ sData[0] + "' \n");
					}
				}
				else
				{
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//					sbQuery.Append(" AND ST04.�X���b�c = CM10.�X���b�c(+) \n");
					sbQuery.Append(" WHERE ST04W.�X���b�c = CM10.�X���b�c(+) \n");
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END
				}

// DEL 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//				//�X��
//				if(sData[2].Length > 0)
//				{
//					if(sData[3].Length > 0)
//					{
//						sbQuery.Append(" AND ST04.�X���b�c  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
//					}
//					else
//					{
//						sbQuery.Append(" AND ST04.�X���b�c = '"+ sData[2] + "' \n");
//					}
//				}
// DEL 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//				// �r�l�O�P�ב��l
//				sbQuery.Append(" AND ST04.����b�c   = SM01.����b�c \n");
//				sbQuery.Append(" AND ST04.����b�c   = SM01.����b�c \n");
//				sbQuery.Append(" AND ST04.�ב��l�b�c = SM01.�ב��l�b�c \n");
//
//				sbQuery.Append(" ORDER BY �o�ד�, �n��P, �X���b�c, ���Ӑ�b�c, ���ۂb�c \n");
				// �b�l�O�P���
				sbQuery.Append(" AND ST04W.����b�c = CM01.����b�c(+) \n");
				// �b�l�O�Q����
				sbQuery.Append(" AND ST04W.����b�c = CM02.����b�c(+) \n");
				sbQuery.Append(" AND ST04W.����b�c = CM02.����b�c(+) \n");

// MOD 2007.02.06 ���s�j���� �K�c�a�e�X�g�G���[�C�� START
//				sbQuery.Append(" ORDER BY �n��P, �n��Q, ST04W.�X���b�c, ST04W.����b�c, ST04W.����b�c \n");
				sbQuery.Append(" ORDER BY �n��P, �n��Q, ST04W.�X���b�c, ST04W.����b�c, ST04W.����b�c \n");
// MOD 2007.02.06 ���s�j���� �K�c�a�e�X�g�G���[�C�� END
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read())
				{
					sbData = new StringBuilder(1024);
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//					sbData.Append(         sDbl + sSng + reader.GetString(0).Trim()              + sDbl);	// �n��b�c
//					sbData.Append(sKanma + sDbl + sSng + reader.GetString(1).Trim()              + sDbl);	// �X���b�c
//					sbData.Append(sKanma + sDbl +        reader.GetString(2).Trim()              + sDbl);	// �X����
//					sbData.Append(sKanma + sDbl +        reader.GetString(4).Trim() + reader.GetString(5).Trim() + sDbl);
//																											// �׎喼
//					sbData.Append(sKanma + sDbl + sSng + reader.GetString(3).Trim()              + sDbl);	// �׎喼�b�c
//					sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim()              + sDbl);	// �o�ד�
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(10).ToString().Trim() + sDbl);	// �f�[�^����
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(11).ToString().Trim() + sDbl);	// ��
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(12).ToString().Trim() + sDbl);	// �d��
//																											// 
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(14).ToString().Trim() + sDbl);	// �^��
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(15).ToString().Trim() + sDbl);	// ���p��
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(16).ToString().Trim() + sDbl);	// �ی���
					sbData.Append(         sDbl + sSng + reader.GetString(0).Trim()              + sDbl);	// �n��P�b�c
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(1).Trim()              + sDbl);	// �n��Q�b�c
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(2).Trim()              + sDbl);	// �X���b�c
					sbData.Append(sKanma + sDbl +        reader.GetString(3).Trim()              + sDbl);	// �X����
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim()              + sDbl);	// ����b�c
					sbData.Append(sKanma + sDbl +        reader.GetString(5).Trim()              + sDbl);	// �X����
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim()              + sDbl);	// ����b�c
					sbData.Append(sKanma + sDbl +        reader.GetString(7).Trim()              + sDbl);	// ���喼

					sbData.Append(sKanma + sDbl +        reader.GetDecimal(8).ToString().Trim() + sDbl);	// ����
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(9).ToString().Trim() + sDbl);	// ��
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(10).ToString().Trim() + sDbl);	// �d��
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(11).ToString().Trim() + sDbl);	// �^��
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(12).ToString().Trim() + sDbl);	// ���p��
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(13).ToString().Trim() + sDbl);	// �ی���
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

					sList.Add(sbData);
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "�Y���f�[�^������܂���";
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * �o�׏󋵈ꗗ�\�i����p�j�Q
		 * �����F������A�n��i�J�n�A�I���j�A�X���i�J�n�A�I���j�A
		 *		 �o�ד��i�J�n�A�I���j
		 * �ߒl�F�X�e�[�^�X�A�X�����A���Ӑ敔�ۖ��A...
		 *
		 *********************************************************************/
		[WebMethod]
		public ArrayList Get_syuuka_Prn2(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�o�׏󋵈ꗗ�\�o�͗p�擾�Q�J�n");

			OracleConnection conn2 = null;
			ArrayList alRet = new ArrayList();

			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				alRet.Insert(0, sRet);
				return alRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				alRet.Insert(0, sRet);
//				return alRet;
//			}

			StringBuilder sbQuery  = new StringBuilder(1024);
			try
			{
				sbQuery.Append(GET_SYUUKA_INF2_SELECT);
				sbQuery.Append("   WHERE ST04.�o�ד� BETWEEN '"+ sData[4] + "' AND '"+ sData[5] +"' \n");

// ADD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
				//�X��
				if(sData[2].Length > 0)
				{
					if(sData[3].Length > 0)
					{
						sbQuery.Append(" AND ST04.�X���b�c  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND ST04.�X���b�c = '"+ sData[2] + "' \n");
					}
				}
				sbQuery.Append(" GROUP BY ST04.�X���b�c, ST04.����b�c, ST04.����b�c \n");
				sbQuery.Append(GET_SYUUKA_INF2_FROM);
// ADD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

				//�n��
				if(sData[0].Length > 0)
				{
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//					sbQuery.Append(" AND ST04.�X���b�c = CM10.�X���b�c \n");
					sbQuery.Append(" WHERE ST04W.�X���b�c = CM10.�X���b�c \n");
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END
					if(sData[1].Length > 0){
						sbQuery.Append(" AND CM10.�n��P  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND CM10.�n��P = '"+ sData[0] + "' \n");
					}
				}
				else
				{
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//					sbQuery.Append(" AND ST04.�X���b�c = CM10.�X���b�c(+) \n");
					sbQuery.Append(" WHERE ST04W.�X���b�c = CM10.�X���b�c(+) \n");
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END
				}

// DEL 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//				//�X��
//				if(sData[2].Length > 0)
//				{
//					if(sData[3].Length > 0)
//					{
//						sbQuery.Append(" AND ST04.�X���b�c  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
//					}
//					else
//					{
//						sbQuery.Append(" AND ST04.�X���b�c = '"+ sData[2] + "' \n");
//					}
//				}
// DEL 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//				// �r�l�O�P�ב��l
//				sbQuery.Append(" AND ST04.����b�c   = SM01.����b�c \n");
//				sbQuery.Append(" AND ST04.����b�c   = SM01.����b�c \n");
//				sbQuery.Append(" AND ST04.�ב��l�b�c = SM01.�ב��l�b�c \n");
//
//				sbQuery.Append(" ORDER BY �o�ד�, �n��P, �X���b�c, ���Ӑ�b�c, ���ۂb�c \n");
				// �b�l�O�P���
				sbQuery.Append(" AND ST04W.����b�c = CM01.����b�c(+) \n");
				// �b�l�O�Q����
				sbQuery.Append(" AND ST04W.����b�c = CM02.����b�c(+) \n");
				sbQuery.Append(" AND ST04W.����b�c = CM02.����b�c(+) \n");

// MOD 2007.02.06 ���s�j���� �K�c�a�e�X�g�G���[�C�� START
//				sbQuery.Append(" ORDER BY �n��P, �n��Q, ST04W.�X���b�c, ST04W.����b�c, ST04W.����b�c \n");
				sbQuery.Append(" ORDER BY �n��P, �n��Q, ST04W.�X���b�c, ST04W.����b�c, ST04W.����b�c \n");
// MOD 2007.02.06 ���s�j���� �K�c�a�e�X�g�G���[�C�� END
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				while (reader.Read())
				{
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX START
//					string[] sbData = new string[12];
//					sbData[0]  = reader.GetString(0).Trim();	// �n��b�c
//					sbData[1]  = reader.GetString(1).Trim();	// �X���b�c
//					sbData[2]  = reader.GetString(2).Trim();	// �X����
//					sbData[3]  = reader.GetString(4).Trim() + reader.GetString(5).Trim();
//																// �׎喼
//					sbData[4]  = reader.GetString(3).Trim();	// �׎喼�b�c
//					sbData[5]  = reader.GetString(9).Trim();	// �o�ד�
//					sbData[6]  = reader.GetDecimal(10).ToString().Trim();	// �f�[�^����
//					sbData[7]  = reader.GetDecimal(11).ToString().Trim();	// ��
//					sbData[8]  = reader.GetDecimal(12).ToString().Trim();	// �d��
//					sbData[9]  = reader.GetDecimal(14).ToString().Trim();	// �^��
//					sbData[10] = reader.GetDecimal(15).ToString().Trim();	// ���p��
//					sbData[11] = reader.GetDecimal(16).ToString().Trim();	// �ی���
					string[] sbData = new string[14];
					sbData[0]  = reader.GetString(0).Trim();	// �n��P�b�c
					sbData[1]  = reader.GetString(1).Trim();	// �n��Q�b�c
					sbData[2]  = reader.GetString(2).Trim();	// �X���b�c
					sbData[3]  = reader.GetString(3).Trim();	// �X����
					sbData[4]  = reader.GetString(4).Trim();	// ����b�c
					sbData[5]  = reader.GetString(5).Trim();	// �����
					sbData[6]  = reader.GetString(6).Trim();	// ����b�c
					sbData[7]  = reader.GetString(7).Trim();	// ���喼

					sbData[8]  = reader.GetDecimal(8).ToString().Trim();	// ����
					sbData[9]  = reader.GetDecimal(9).ToString().Trim();	// ��
					sbData[10]  = reader.GetDecimal(10).ToString().Trim();	// �d��
					sbData[11]  = reader.GetDecimal(11).ToString().Trim();	// �^��
					sbData[12] = reader.GetDecimal(12).ToString().Trim();	// ���p��
					sbData[13] = reader.GetDecimal(13).ToString().Trim();	// �ی���
// MOD 2007.02.01 ���s�j���� �o�׏󋵈ꗗ�\�̕ύX END
					alRet.Add(sbData);
				}
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

				if (alRet.Count == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
					alRet.Add(sRet);
				}
				else
				{
					sRet[0] = "����I��";
					alRet.Insert(0, sRet);
				}

			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
				alRet.Insert(0, sRet);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
				alRet.Insert(0, sRet);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			return alRet;
		}

// ADD 2006.12.20 ���s�j���� �o�׏󋵈ꗗ�\�̋@�\�ǉ� END
// ADD 2007.01.12 ���s�j���� ���˗��匟���̃R�s�[ START
		/*********************************************************************
		 * �˗���f�[�^�擾
		 * �����F����b�c�A����b�c�A�ב��l�b�c�A�X���b�c
		 * �ߒl�F�X�e�[�^�X�A�J�i���́A�d�b�ԍ��A�X�֔ԍ��A�Z���A���O�A�d��
		 *		 ���[���A�h���X�A���Ӑ�b�c�A���Ӑ敔�ۂb�c�A�X�V����
		 *********************************************************************/
		private static string GET_SIRAINUSI_SELECT1
			= "SELECT SM01.���O�P \n"
			+ " FROM �r�l�O�P�ב��l SM01 \n"
			+ ", �b�l�O�Q���� CM02 \n"
			+ ", �b�l�P�S�X�֔ԍ� CM14 \n"
			+ "";

		private static string GET_SIRAINUSI_SELECT2
			= "SELECT CM02.���喼 \n"
			+ " FROM �b�l�O�Q���� CM02 \n"
			+ ", �b�l�P�S�X�֔ԍ� CM14 \n"
			+ "";

		private static string GET_SIRAINUSI_SELECT3
			= "SELECT CM01.����� \n"
			+ " FROM �b�l�O�P��� CM01 \n"
			+ ", �b�l�O�Q���� CM02 \n"
			+ ", �b�l�P�S�X�֔ԍ� CM14 \n"
			+ "";

		[WebMethod]
		public String[] Get_Sirainusi(string[] sUser, string sKCode, string sBCode, string sICode, string sTCode)
		{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//			logFileOpen(sUser);
			logWriter(sUser, INF, "�˗�����擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[4]{"","","",""};

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//
//			// ����`�F�b�N
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			try
			{
				string cmdQuery = "";
				OracleDataReader reader;

				if(sKCode.Length > 0){
					cmdQuery = GET_SIRAINUSI_SELECT3
						+ " WHERE CM01.����b�c = '" + sKCode + "' \n"
						+ " AND CM01.�폜�e�f = '0' \n"
						+ " AND CM01.����b�c = CM02.����b�c \n"
						+ " AND CM02.�폜�e�f = '0' \n"
						+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
						+ "";

					//�X���b�c���ݒ肳��Ă��鎞
					if(sTCode.Length > 0){
						cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
					}

					reader = CmdSelect(sUser, conn2, cmdQuery);

					if(reader.Read()) sRet[1]  = reader.GetString(0).Trim();
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
					if(sBCode.Length > 0){
						cmdQuery = GET_SIRAINUSI_SELECT2
							+ " WHERE CM02.����b�c = '" + sKCode + "' \n"
							+ " AND CM02.����b�c = '" + sBCode + "' \n"
							+ " AND CM02.�폜�e�f = '0' \n"
							+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
							+ "";

						//�X���b�c���ݒ肳��Ă��鎞
						if(sTCode.Length > 0){
							cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
						}

						reader = CmdSelect(sUser, conn2, cmdQuery);

						if(reader.Read()) sRet[2]  = reader.GetString(0).Trim();
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
						disposeReader(reader);
						reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END

						if(sICode.Length > 0){
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
								+ " AND SM01.����b�c = '" + sBCode + "' \n"
								+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
								+ " AND SM01.�폜�e�f = '0' \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND CM02.�폜�e�f = '0' \n"
								+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
								+ "";

							//�X���b�c���ݒ肳��Ă��鎞
							if(sTCode.Length > 0){
								cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
							disposeReader(reader);
							reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
						}
					}else{
						//����b�c�������͂̏ꍇ
						if(sICode.Length > 0){
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
								+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
								+ " AND SM01.�폜�e�f = '0' \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND CM02.�폜�e�f = '0' \n"
								+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
								+ "";

							//�X���b�c���ݒ肳��Ă��鎞
							if(sTCode.Length > 0){
								cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
							disposeReader(reader);
							reader = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
						}
					}
				}

				sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� START
				conn2 = null;
// ADD 2007.04.28 ���s�j���� �I�u�W�F�N�g�̔j�� END
// DEL 2007.05.10 ���s�j���� ���g�p�֐��̃R�����g��
//				logFileClose();
			}
			
			return sRet;
		}
// ADD 2007.01.12 ���s�j���� ���˗��匟���̃R�s�[ END

// ADD 2007.11.14 KCL) �X�{ global�Ή��ׁ̈A���q�l�R�[�h�m�F���R�s�[ START
		/*********************************************************************
		 * �˗�����擾�Q
		 * �����F���[�U�[�A����b�c�A����b�c�A�ב��l�b�c�A�X���b�c
		 * �ߒl�F�˗�����
		 *
		 * �Q�ƌ��F�o�׏Ɖ�.cs
		 *********************************************************************/
		[WebMethod]
		public String[] Get_Sirainusi2(string[] sUser, string sKCode, string sBCode, string sICode, string sTCode)
		{
			logWriter(sUser, INF, "�˗�����擾�Q�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[4]{"","","",""};

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			try
			{
				string cmdQuery = "";
				OracleDataReader reader;

				if(sKCode.Length > 0)
				{
					cmdQuery = GET_SIRAINUSI_SELECT3
						+ " WHERE CM01.����b�c = '" + sKCode + "' \n"
						+ " AND CM01.�폜�e�f = '0' \n"
						+ " AND CM01.����b�c = CM02.����b�c \n"
						+ " AND CM02.�폜�e�f = '0' \n"
						+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
						+ "";

					//�X���b�c���ݒ肳��Ă��鎞
					if(sTCode.Length > 0)
					{
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� START
						cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
//						cmdQuery += " AND CM14.�X���b�c IN ('" + sTCode + "','030') \n";
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� END
					}
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//					cmdQuery += " AND CM14.�폜�e�f = '0' \n";
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
					//�X���b�c���ݒ肳��Ă��鎞
					if (sTCode.Length > 0) 
					{
						cmdQuery += "UNION \n";
						cmdQuery += "SELECT CM01.����� \n"
							+ " FROM �b�l�O�P��� CM01 \n"
							+ "     ,�b�l�O�T������X CM05 \n"
							+ " WHERE CM01.����b�c = '" + sKCode + "' \n"
							+ " AND CM01.�폜�e�f = '0' \n"
							+ " AND CM01.����b�c = CM05.����b�c \n"
							+ " AND CM05.�폜�e�f = '0' \n"
							+ " AND CM05.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
						cmdQuery += "UNION \n";
						cmdQuery += "SELECT CM01.����� \n"
							+ " FROM �b�l�O�P��� CM01 \n"
							+ "     ,�b�l�O�T������X�e CM05F \n"
							+ " WHERE CM01.����b�c = '" + sKCode + "' \n"
							+ " AND CM01.�폜�e�f = '0' \n"
							+ " AND CM01.����b�c = CM05F.����b�c \n"
							+ " AND CM05F.�폜�e�f = '0' \n"
							+ " AND CM05F.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
					}

					reader = CmdSelect(sUser, conn2, cmdQuery);

					if(reader.Read()) sRet[1]  = reader.GetString(0).Trim();
					disposeReader(reader);
					reader = null;

					if(sBCode.Length > 0)
					{
						cmdQuery = GET_SIRAINUSI_SELECT2
							+ " WHERE CM02.����b�c = '" + sKCode + "' \n"
							+ " AND CM02.����b�c = '" + sBCode + "' \n"
							+ " AND CM02.�폜�e�f = '0' \n"
							+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
							+ "";

						//�X���b�c���ݒ肳��Ă��鎞
						if(sTCode.Length > 0)
						{
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� START
							cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
//							cmdQuery += " AND CM14.�X���b�c IN ('" + sTCode + "','030') \n";
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� END
						}

// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//						cmdQuery += " AND CM14.�폜�e�f = '0' \n";
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
						//�X���b�c���ݒ肳��Ă��鎞
						if (sTCode.Length > 0) 
						{
							cmdQuery += "UNION \n";
							cmdQuery += "SELECT CM02.���喼 \n"
								+ " FROM �b�l�O�Q���� CM02 \n"
								+ "     ,�b�l�O�T������X CM05 \n"
								+ " WHERE CM02.����b�c = '" + sKCode + "' \n"
								+ " AND CM02.����b�c = '" + sBCode + "' \n"
								+ " AND CM02.�폜�e�f = '0' \n"
								+ " AND CM02.����b�c = CM05.����b�c \n"
								+ " AND CM05.�폜�e�f = '0' \n"
								+ " AND CM05.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
							cmdQuery += "UNION \n";
							cmdQuery += "SELECT CM02.���喼 \n"
								+ " FROM �b�l�O�Q���� CM02 \n"
								+ "     ,�b�l�O�T������X�e CM05F \n"
								+ " WHERE CM02.����b�c = '" + sKCode + "' \n"
								+ " AND CM02.����b�c = '" + sBCode + "' \n"
								+ " AND CM02.�폜�e�f = '0' \n"
								+ " AND CM02.����b�c = CM05F.����b�c \n"
								+ " AND CM05F.�폜�e�f = '0' \n"
								+ " AND CM05F.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
						}

						reader = CmdSelect(sUser, conn2, cmdQuery);

						if(reader.Read()) sRet[2]  = reader.GetString(0).Trim();
						disposeReader(reader);
						reader = null;

						if(sICode.Length > 0)
						{
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
								+ " AND SM01.����b�c = '" + sBCode + "' \n"
								+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
								+ " AND SM01.�폜�e�f = '0' \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND CM02.�폜�e�f = '0' \n"
								+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
								+ "";

							//�X���b�c���ݒ肳��Ă��鎞
							if(sTCode.Length > 0)
							{
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� START
								cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
//								cmdQuery += " AND CM14.�X���b�c IN ('" + sTCode + "','030') \n";
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� END
							}

// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//							cmdQuery += " AND CM14.�폜�e�f = '0' \n";
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
							//�X���b�c���ݒ肳��Ă��鎞
							if (sTCode.Length > 0) 
							{
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.���O�P \n"
									+ " FROM �r�l�O�P�ב��l SM01 \n"
									+ "     ,�b�l�O�T������X CM05 \n"
									+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
									+ " AND SM01.����b�c = '" + sBCode + "' \n"
									+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
									+ " AND SM01.�폜�e�f = '0' \n"
									+ " AND SM01.����b�c = CM05.����b�c \n"
									+ " AND CM05.�폜�e�f = '0' \n"
									+ " AND CM05.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.���O�P \n"
									+ " FROM �r�l�O�P�ב��l SM01 \n"
									+ "     ,�b�l�O�T������X�e CM05F \n"
									+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
									+ " AND SM01.����b�c = '" + sBCode + "' \n"
									+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
									+ " AND SM01.�폜�e�f = '0' \n"
									+ " AND SM01.����b�c = CM05F.����b�c \n"
									+ " AND CM05F.�폜�e�f = '0' \n"
									+ " AND CM05F.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
							disposeReader(reader);
							reader = null;
						}
					}
					else
					{
						//����b�c�������͂̏ꍇ
						if(sICode.Length > 0)
						{
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
								+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
								+ " AND SM01.�폜�e�f = '0' \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND SM01.����b�c = CM02.����b�c \n"
								+ " AND CM02.�폜�e�f = '0' \n"
								+ " AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
								+ "";

							//�X���b�c���ݒ肳��Ă��鎞
							if(sTCode.Length > 0)
							{
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� START
								cmdQuery += " AND CM14.�X���b�c = '" + sTCode + "' \n";
//							cmdQuery += " AND CM14.�X���b�c IN ('" + sTCode + "','030') \n";
//�ۗ� MOD 2010.07.21 ���s�j���� ���R�[�l�Ή� END
							}

// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� START
//							cmdQuery += " AND CM14.�폜�e�f = '0' \n";
// MOD 2010.04.13 ���s�j���� �X�֔ԍ����폜���ꂽ���̏�Q�Ή� END
							//�X���b�c���ݒ肳��Ă��鎞
							if (sTCode.Length > 0) 
							{
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.���O�P \n"
									+ " FROM �r�l�O�P�ב��l SM01 \n"
									+ "     ,�b�l�O�T������X CM05 \n"
									+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
									+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
									+ " AND SM01.�폜�e�f = '0' \n"
									+ " AND SM01.����b�c = CM05.����b�c \n"
									+ " AND CM05.�폜�e�f = '0' \n"
									+ " AND CM05.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.���O�P \n"
									+ " FROM �r�l�O�P�ב��l SM01 \n"
									+ "     ,�b�l�O�T������X�e CM05F \n"
									+ " WHERE SM01.����b�c = '" + sKCode + "' \n"
									+ " AND SM01.�ב��l�b�c = '" + sICode + "' \n"
									+ " AND SM01.�폜�e�f = '0' \n"
									+ " AND SM01.����b�c = CM05F.����b�c \n"
									+ " AND CM05F.�폜�e�f = '0' \n"
									+ " AND CM05F.�X���b�c = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
							disposeReader(reader);
							reader = null;
						}
					}
				}

				sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2007.11.14 KCL) �X�{ global�Ή��ׁ̈A���q�l�R�[�h�m�F���R�s�[ END
// ADD 2008.02.14 ���s�j���� �Z�b�V�������̎擾 START
		/*********************************************************************
		 * �c�a�Z�b�V�������̎擾
		 * �����F���[�U�[
		 * �ߒl�F�c�a�Z�b�V�������
		 *
		 *********************************************************************/
		private static string GET_DBSESSIONCOUNT_CONNECT
			= "User Id=SYSTEM;Password=MANAGER;Data Source="
			+ "IS2DB01;Pooling=false;Connection Timeout=180"
			;
		private static string GET_DBSESSIONCOUNT_SELECT
			= "SELECT TERMINAL, USERNAME, PROGRAM, STATUS, COUNT(*) \n"
			+ "FROM V$SESSION \n"
			+ "GROUP BY TERMINAL, USERNAME, PROGRAM, STATUS \n"
			+ "ORDER BY TERMINAL, USERNAME, PROGRAM, STATUS \n"
			;
		private static int GET_DBSESSIONCOUNT_ROWS = 20;
		[WebMethod]
		public string[][] Get_DBSessionCount(string[] sUser)
		{
			logWriter(sUser, INF, "�Z�b�V�������̎擾");

			string[][] sRet;
			sRet = new string[GET_DBSESSIONCOUNT_ROWS][];
			sRet[0] = new string[]{""};

			OracleConnection conn   = null;
			OracleDataReader reader = null;

			try
			{
				// �c�a�ڑ�
				conn = new OracleConnection(GET_DBSESSIONCOUNT_CONNECT);
				conn.Open();

				OracleCommand cmd = new OracleCommand(GET_DBSESSIONCOUNT_SELECT);
				cmd.Connection  = conn;
				cmd.CommandType = CommandType.Text;

				cmd.Prepare();
				reader = cmd.ExecuteReader();
				cmd.Dispose();

				sRet = new string[GET_DBSESSIONCOUNT_ROWS][];
				sRet[0] = new string[]{""};
				int iRow = 1;
				while(reader.Read()){
					sRet[iRow] = new string[reader.FieldCount];
					for(int iCol = 0; iCol < reader.FieldCount; iCol++)
					{
						sRet[iRow][iCol] = reader.GetValue(iCol).ToString();
					}
					iRow++;
					if(iRow >= GET_DBSESSIONCOUNT_ROWS) break;
				}
			}
			catch (OracleException ex)
			{
				sRet[0][0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0][0] = ex.Message;
				logWriter(sUser, ERR, "�T�[�o�G���[�F" + ex.Message);
			}
			finally
			{
				if(reader != null){
					try{ reader.Close(); } 
					catch (Exception){};
					try{ reader.Dispose(); } 
					catch (Exception){};
					reader = null;
				}

				try{
					conn.Close();
				}catch (Exception ex){
					logWriter(sUser, ERR, "�ؒf�G���[�F" + ex.Message);
				}
				try{
					conn.Dispose();
				}catch (Exception ex){
					logWriter(sUser, ERR, "�j���G���[�F" + ex.Message);
				}
				conn = null;
			}

			return sRet;
		}
// ADD 2008.02.14 ���s�j���� �Z�b�V�������̎擾 END
// ADD 2009.01.06 ���s�j���� �p�X���[�h�`�F�b�N�Ή� START
		/*********************************************************************
		 * �p�X���[�h�X�V�����X�g�擾
		 * �����F���[�U�[���A�p�X���[�h�X�V���J�n�A�I��
		 * �ߒl�F�p�X���[�h�X�V�����X�g
		 *
		 *********************************************************************/
		private static string GET_PASSUPDDATE_SELECT1
			= "SELECT CM04.�o�^�o�f, COUNT(*) \n"
			+ " FROM �b�l�O�P��� CM01 \n"
			+ " , �b�l�O�Q���� CM02 \n"
			+ " , �b�l�O�S���p�� CM04 \n"
			+ " WHERE CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
			+ " AND CM01.����b�c = CM02.����b�c \n"
			+ " AND CM02.����b�c = CM04.����b�c \n"
			+ " AND CM02.����b�c = CM04.����b�c \n"
			+ " AND CM01.�폜�e�f = '0' \n"
			+ " AND CM02.�폜�e�f = '0' \n"
			;

		private static string GET_PASSUPDDATE_WHERE1
			= " AND CM04.�폜�e�f = '0' \n"
			+ " GROUP BY CM04.�o�^�o�f \n"
			+ " ORDER BY CM04.�o�^�o�f \n"
			;

		[WebMethod]
		public String[] Get_PassUpdDate(string[] sUser, string sDateS, string sDateE)
		{
			logWriter(sUser, INF, "�p�X���[�h�X�V�����X�g�擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[32];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbRet = new StringBuilder(1024);
			try
			{
				OracleDataReader reader;

				sbQuery.Append(GET_PASSUPDDATE_SELECT1);
				if(sDateE == null || sDateE.Length == 0){
					sbQuery.Append(" AND CM04.�o�^�o�f = '"+sDateS+"' \n");
				}else if(sDateS == null || sDateS.Length == 0){
					sbQuery.Append(" AND CM04.�o�^�o�f <= '"+sDateE+"' \n");
				}else{
					sbQuery.Append(" AND CM04.�o�^�o�f BETWEEN '"+sDateS+"' AND '"+sDateE+"' \n");
				}
				sbQuery.Append(GET_PASSUPDDATE_WHERE1);

				reader = CmdSelect(sUser, conn2, sbQuery);

				int iCnt = 1;
				while(reader.Read() && iCnt < sRet.Length){
					sbRet = new StringBuilder(1024);
					sbRet.Append(sSepa + reader.GetString(0).Trim());
					sbRet.Append(sSepa + reader.GetString(1).Trim());
					sbRet.Append(sSepa);
					sRet[iCnt] = sbRet.ToString();
					iCnt++;
				}
				disposeReader(reader);
				reader = null;

				sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				sbQuery = null;
				sbRet = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}

		/*********************************************************************
		 * �p�X���[�h�X�V���ꊇ�X�V
		 * �����F���[�U�[���A�p�X���[�h�X�V�����A�p�X���[�h�X�V���V�A����
		 * �ߒl�F����
		 *
		 *********************************************************************/
		private static string UPD_PASSUPDDATE_WHERE1
			= " WHERE (����b�c, ���p�҂b�c) \n"
			+ " IN ( \n"
			+ " SELECT CM04.����b�c, CM04.���p�҂b�c \n"
			+ " FROM �b�l�O�P��� CM01 \n"
			+ " , �b�l�O�Q���� CM02 \n"
			+ " , �b�l�O�S���p�� CM04 \n"
			+ " WHERE CM01.�g�p�I���� >= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
			+ " AND CM01.����b�c = CM02.����b�c \n"
			+ " AND CM02.����b�c = CM04.����b�c \n"
			+ " AND CM02.����b�c = CM04.����b�c \n"
			+ " AND CM01.�폜�e�f = '0' \n"
			+ " AND CM02.�폜�e�f = '0' \n"
			;
		private static string UPD_PASSUPDDATE_WHERE2
			= " AND CM04.�폜�e�f = '0' \n"
			+ " ) \n"
			;

		[WebMethod]
		public String[] Upd_PassUpdDate(string[] sUser, string sDateOld, string sDateNew, int iNum)
		{
			logWriter(sUser, INF, "�p�X���[�h�X�V���ꊇ�X�V�J�n");
			string[] sRet = new string[1]{""};
			OracleConnection conn2 = null;

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();
			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
				sbQuery.Append("UPDATE �b�l�O�S���p�� \n");
				sbQuery.Append(" SET �o�^�o�f = '"+sDateNew+"' \n");
				sbQuery.Append(" , �X�V�o�f = '�p�X�ꊇ' \n");
				sbQuery.Append(" , �X�V�� = '"+sUser[1]+"' \n");

				sbQuery.Append(UPD_PASSUPDDATE_WHERE1);
				sbQuery.Append(" AND CM04.�o�^�o�f = '"+sDateOld+"' \n");
				sbQuery.Append(UPD_PASSUPDDATE_WHERE2);
				if(iNum > 0){
					sbQuery.Append(" AND ROWNUM <= "+iNum+" \n");
				}

				CmdUpdate(sUser, conn2, sbQuery);

				sRet[0] = "����I��";
				tran.Commit();
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				sbQuery = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2009.01.06 ���s�j���� �p�X���[�h�`�F�b�N�Ή� END
//�ۗ��@���q�l���X�g�̍쐬
// ADD 2009.04.02 ���s�j���� �ғ����Ή� START
		/*********************************************************************
		 * �ғ����擾
		 * �����F���[�U�[���A�J�n���A�I����
		 * �ߒl�F����
		 *
		 *********************************************************************/
		private static string GET_KADOBI_SELECT
			= " SELECT CM07.�N����, CM07.�ғ����e�f, CM07.���̑��e�f \n"
			+ " FROM �b�l�O�V�ғ��� CM07 \n"
			;

		[WebMethod]
		public object[] Get_Kadobi(string[] sUser, string sDateStart, string sDateEnd)
		{
			logWriter(sUser, INF, "�ғ����擾�J�n");
			string   sRet  = "";
			string[] sKadouFG = new string[32];
			string[] sOtherFG = new string[32];
			OracleConnection conn2 = null;

			int iCnt;
			for(iCnt = 0; iCnt < sKadouFG.Length; iCnt++){
				sKadouFG[iCnt] = "";
				sOtherFG[iCnt] = "";
			}

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet = "�c�a�ڑ��G���[";
				return new object[]{sRet};
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
				OracleDataReader reader;

				sbQuery.Append(GET_KADOBI_SELECT);
				sbQuery.Append(" WHERE CM07.�N���� >= " + sDateStart + " \n");
				sbQuery.Append(  " AND CM07.�N���� <= " + sDateEnd + " \n");
				sbQuery.Append(  " AND CM07.�폜�e�f = '0' \n");
				sbQuery.Append(" ORDER BY CM07.�N���� \n");

				reader = CmdSelect(sUser, conn2, sbQuery);

				iCnt = 1;
				while(reader.Read() && iCnt < sKadouFG.Length){
					sKadouFG[iCnt]  = reader.GetString(1);
					sOtherFG[iCnt]  = reader.GetString(2);
					iCnt++;
				}

				disposeReader(reader);
				reader = null;
				sRet = "����I��";
				logWriter(sUser, INF, sRet);
			}
			catch (OracleException ex)
			{
				sRet = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet);
			}
			finally
			{
				sbQuery = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return new object[]{sRet, sKadouFG, sOtherFG};
		}

		/*********************************************************************
		 * �ғ����X�V
		 * �����F���[�U�[���A�J�n���A�f�[�^�i���P�������𒴂��Ȃ����Ɓj
		 * �ߒl�F����
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_Kadobi(string[] sUser, string sDateStart, char[] cKadouFG, char[] cOtherFG)
		{
			logWriter(sUser, INF, "�ғ����X�V�J�n");
			string[] sRet = new string[1]{""};
			OracleConnection conn2 = null;
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();
			StringBuilder sbQuery;
			try
			{
				for(int iCnt = 0; iCnt < cKadouFG.Length; iCnt++){
					sbQuery = new StringBuilder(1024);
					sbQuery.Append("UPDATE �b�l�O�V�ғ��� \n");
					sbQuery.Append(" SET �ғ����e�f = '"+ cKadouFG[iCnt] +"' \n");
					sbQuery.Append(" ,���̑��e�f = '"+ cOtherFG[iCnt] +"' \n");
					sbQuery.Append(" ,�폜�e�f = '0' \n");
					sbQuery.Append(" ,�X�V���� = "+ s�X�V���� +" \n");
					sbQuery.Append(" ,�X�V�o�f = '�ғ��X�V' \n");
					sbQuery.Append(" ,�X�V�� = '"+ sUser[1] +"' \n");
					sbQuery.Append(" WHERE �N���� = "+ sDateStart +" + "+ iCnt +" \n");

					int iUpdCnt = CmdUpdate(sUser, conn2, sbQuery);
					if(iUpdCnt == 0){
						sbQuery = new StringBuilder(1024);
						sbQuery.Append("INSERT INTO �b�l�O�V�ғ��� VALUES( \n");
						sbQuery.Append(" "+ sDateStart +" + "+ iCnt +" \n");
						sbQuery.Append(" ,'"+ cKadouFG[iCnt] +"' \n");
						sbQuery.Append(" ,'"+ cOtherFG[iCnt] +"' \n");
						sbQuery.Append(" ,'0' \n");
						sbQuery.Append(" ,"+ s�X�V���� +" \n");
						sbQuery.Append(" ,'�ғ��X�V' \n");
						sbQuery.Append(" ,'"+ sUser[1] +"' \n");
						sbQuery.Append(" ,"+ s�X�V���� +" \n");
						sbQuery.Append(" ,'�ғ��X�V' \n");
						sbQuery.Append(" ,'"+ sUser[1] +"' \n");
						sbQuery.Append(" ) \n");
						CmdUpdate(sUser, conn2, sbQuery);
					}
				}

				sRet[0] = "����I��";
				tran.Commit();
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				sbQuery = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2009.04.02 ���s�j���� �ғ����Ή� END
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� START
		/*********************************************************************
		 * ����}�X�^�X�V�Q
		 * �����F����b�c�A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Member2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "����}�X�^�X�V�Q�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�P��� \n"
					+   " SET �L���A�g�e�f = '" + sKey[1] + "' "
					+       ",�X�V���� = " + s�X�V����
					+       ",�X�V�o�f = '" + sKey[3] + "' "
					+       ",�X�V�� = '" + sKey[4] + "' \n"
					+ " WHERE ����b�c = '" + sKey[0] + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� = " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
					sRet[1] = s�X�V����;
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2009.05.28 ���s�j���� �o�׎��шꗗ�^����\���Ή� END
// MOD 2009.07.09 ���s�j���� �z����񌟍��@�\�̒ǉ� START
		/*********************************************************************
		 * �z����񌟍�
		 * �����F����b�c�A�����...
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Haikan(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�z����񌟍��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[30];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				OracleDataReader reader;

				cmdQuery
					= "SELECT * FROM �f�s�O�Q�z�� \n"
					+ " WHERE ���[�ԍ� = '" + sKey[0] + "' \n"
					;

				reader = CmdSelect(sUser, conn2, cmdQuery);
				if(reader.Read())
				{									 // ���[�ԍ�
					sRet[01] = reader.GetString(01); // �W�דX�b�c
					sRet[02] = reader.GetString(02); // �W�ד�
					sRet[03] = reader.GetString(03); // �W�׎���
					sRet[04] = reader.GetString(04); // �����X�b�c
					sRet[05] = reader.GetString(05); // ������
					sRet[06] = reader.GetString(06); // �����X�b�c
					sRet[07] = reader.GetString(07); // ������
					sRet[08] = reader.GetString(08); // ��������
					sRet[09] = reader.GetString(09); // ���o�X�b�c
					sRet[10] = reader.GetString(10); // ���o��
// MOD 2009.07.09 ���s�j���� �z����񌟍��@�\�̒ǉ� START
					sRet[11] = reader.GetString(11); // ���o����
					sRet[12] = reader.GetString(12); // �z���X�b�c
					sRet[13] = reader.GetString(13); // �z����
					sRet[14] = reader.GetString(14); // �z������
					sRet[15] = reader.GetString(15); // ���R�b�c
					sRet[16] = reader.GetString(16); // �폜�e�f
					sRet[17] = reader.GetDecimal(17).ToString(); // �o�^����
					sRet[18] = reader.GetString(18); // �o�^�o�f
					sRet[19] = reader.GetString(19); // �o�^��
					sRet[20] = reader.GetDecimal(20).ToString(); // �X�V����
					sRet[21] = reader.GetString(21); // �X�V�o�f
					sRet[22] = reader.GetString(22); // �X�V��
// MOD 2009.07.09 ���s�j���� �z����񌟍��@�\�̒ǉ� END
				}
				disposeReader(reader);
				reader = null;
				sRet[0] = "����I��";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2009.07.09 ���s�j���� �z����񌟍��@�\�̒ǉ� END
// MOD 2010.04.30 ���s�j���� �b�r�u�o�͋@�\�̒ǉ� START
		/*********************************************************************
		 * �o�׈ꗗ�擾�i�b�r�u�o�͗p�j
		 * �����F����b�c�A����b�c�A�׎�l�b�c�A�ב��l�b�c�A�o�ד� or �o�^���A
		 *		 �J�n���A�I�����A���
		 * �ߒl�F�X�e�[�^�X�A�o�^���A�W���[�i���m�n�A�׎�l�b�c...
		 *
		 *********************************************************************/
		private static string GET_CSVWRITE3_SELECT
			= "SELECT S.�o�^��, S.�o�ד�, �����ԍ�, S.�׎�l�b�c, S.�X�֔ԍ�, \n"
			+ " '(' || TRIM(S.�d�b�ԍ��P) || ')' || TRIM(S.�d�b�ԍ��Q) || '-' || S.�d�b�ԍ��R, \n"
			+ " S.�Z���P, S.�Z���Q, S.�Z���R, S.���O�P, S.���O�Q, S.����v, S.���X�b�c, S.���X��, \n"
			+ " S.�ב��l�b�c, NVL(SM01.�X�֔ԍ�, ' '), \n"
			+ " NVL(SM01.�d�b�ԍ��P,' '), NVL(SM01.�d�b�ԍ��Q,' '), NVL(SM01.�d�b�ԍ��R,' '), \n"
			+ " NVL(SM01.�Z���P,' '), NVL(SM01.�Z���Q,' '), NVL(SM01.���O�P,' '), NVL(SM01.���O�Q,' '), \n"
			+ " TO_CHAR(S.��), TO_CHAR(S.�d��), \n"
			+ " S.�w���, S.�A���w���P, S.�A���w���Q, S.�i���L���P, S.�i���L���Q, S.�i���L���R, \n"
			+ " S.�����敪, TO_CHAR(S.�ی����z), S.���q�l�o�הԍ�, \n"
			+ " S.���Ӑ�b�c, S.���ۂb�c, S.�ː�, \n"
			+ " S.����b�c, S.����b�c, S.\"�W���[�i���m�n\" \n"
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
			+ ", S.�^���ː�, S.�^���d�� \n"
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			+ ", NVL(CM01.�ۗ�����e�f,'0') \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ� START
			+ ", DECODE(S.�����O�R,'          ',' ',('20' || SUBSTR(S.�����O�R,1,2) || '/' || SUBSTR(S.�����O�R,3,2) || '/' || SUBSTR(S.�����O�R,5,2) || ' ' || SUBSTR(S.�����O�R,7,2) || ':' || SUBSTR(S.�����O�R,9,2))) \n"
// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ� END
			;
		private static string GET_CSVWRITE3_FROM_1
//			= " FROM \"�r�s�O�P�o�׃W���[�i��\" S , �r�l�O�P�ב��l SM01 \n"
			= " FROM \"�r�s�O�P�o�׃W���[�i��\" ST01 , �r�l�O�P�ב��l SM01 \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			+ ", �b�l�O�P��� CM01 \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
			;
		private static string GET_CSVWRITE3_FROM_2 
//			= " FROM \"�r�s�O�Q�o�ח���\"       S , �r�l�O�P�ב��l SM01 \n"
			= " FROM \"�r�s�O�Q�o�ח���\"       ST02 , �r�l�O�P�ב��l SM01 \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			+ ", �b�l�O�P��� CM01 \n"
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
			;
		private static string GET_CSVWRITE3_SORT_1
			= " ORDER BY S.�o�ד�, S.����b�c, S.����b�c, S.�o�^��, S.\"�W���[�i���m�n\" "
			;
		private static string GET_CSVWRITE3_SORT_2
			= " ORDER BY S.����b�c, S.����b�c, S.�o�^��, S.\"�W���[�i���m�n\" "
			;

		[WebMethod]
		public String[] Get_csvwrite3(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�b�r�u�o�͗p�擾�R�J�n");
			string s����b�c   = sKey[0];
			string s����b�c   = sKey[1];
			string s�ב��l�b�c = sKey[2];
			string s���t�敪   = sKey[3];
			string s�J�n��     = sKey[4];
			string s�I����     = sKey[5];
			string s��Ԃb�c   = sKey[6];
			bool   b���o��     = sKey[6].Equals("90");
			string s�����ԍ� = sKey[7];
			string s�폜�e�f   = sKey[8];
			string s���X�b�c   = sKey[9];
			string s�b�r�u�o�͌`�� = ""; if(sKey.Length > 10) s�b�r�u�o�͌`�� = sKey[10];
// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ� START
			string s�z���r�o�͌`�� = ""; if(sKey.Length > 11) s�z���r�o�͌`�� = sKey[11];
// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ� END
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
//�ۗ�			bool   b�^���G���[ = sKey[6].Equals("91");
//�ۗ�			if(b�^���G���[) s�폜�e�f = "1";
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			decimal d�ː� = 0;
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
			string  s�^���ː� = "";
			string  s�^���d�� = "";
			decimal d�d�� = 0;
			decimal d�ː��d�� = 0;
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
			string  s�d�ʓ��͐��� = "0";
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			try
			{
				if(s�����ԍ�.Length == 0){
					if(b���o��){
						sbQuery.Append(", �r�s�O�T���o�ו� ST05 \n");
						if(s����b�c.Length > 0){
							sbQuery.Append(" WHERE ST05.����b�c = '" + s����b�c + "' \n");
						}else{
							sbQuery.Append(" WHERE ST05.����b�c > ' ' \n");
						}
						if(s����b�c.Length > 0){
							sbQuery.Append(" AND ST05.����b�c = '" + s����b�c + "' \n");
						}
						if(s���t�敪 == "0"){
							sbQuery.Append(" AND ST05.�o�ד�  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
						}else{
							sbQuery.Append(" AND ST05.�o�^��  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
						}
						sbQuery.Append(" AND ST05.�o�ד� < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
						sbQuery.Append(" AND ST05.���   = '02' \n");
						if(sKey[9].Length > 0){
							sbQuery.Append(" AND ST05.���X�b�c = '"+ s���X�b�c + "' \n");
						}
						sbQuery.Append(" AND ST05.����b�c = S.����b�c \n");
						sbQuery.Append(" AND ST05.����b�c = S.����b�c \n");
						sbQuery.Append(" AND ST05.�o�^�� = S.�o�^�� \n");
						sbQuery.Append(" AND ST05.\"�W���[�i���m�n\" = S.\"�W���[�i���m�n\" \n");
					}else{
						if(s����b�c.Length > 0){
							sbQuery.Append(" WHERE S.����b�c = '" + s����b�c + "' \n");
						}else{
							sbQuery.Append(" WHERE S.����b�c > ' ' \n");
						}
						if(s����b�c.Length > 0){
							sbQuery.Append(" AND S.����b�c = '" + s����b�c + "' \n");
						}
					}

					if(s�ב��l�b�c.Length > 0){
						sbQuery.Append(" AND S.�ב��l�b�c = '"+ s�ב��l�b�c + "' \n");
					}
					if(s���t�敪 == "0"){
						sbQuery.Append(" AND S.�o�ד�  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
					}else{
						sbQuery.Append(" AND S.�o�^��  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
					}
					
					if(s��Ԃb�c != "00"){
						if(s��Ԃb�c == "aa"){
							sbQuery.Append(" AND S.��� <> '01' \n");
						}else if(b���o��){
							sbQuery.Append(" AND S.�o�ד� < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
							sbQuery.Append(" AND S.���   = '02' \n");
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
//�ۗ�						}else if(b�^���G���[){
//�ۗ�							sbQuery.Append(" AND ( S.�^�� > 0 OR S.���p > 0 OR S.������ > 0 ) \n");
//�ۗ� MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
						}else{
							sbQuery.Append(" AND S.��� = '"+ s��Ԃb�c + "' \n");
						}
					}
				}else{
					sbQuery.Append(" WHERE S.�����ԍ� = '"+ s�����ԍ� + "' \n");
				}
				if(s�폜�e�f != "0"){
					if(s�폜�e�f == "1"){
						sbQuery.Append(" AND S.�폜�e�f = '1' \n");
					}else{
						sbQuery.Append(" AND S.�폜�e�f = '0' \n");
					}
				}
				if(s���X�b�c.Length > 0){
					sbQuery.Append(" AND S.���X�b�c = '"+ s���X�b�c + "' \n");
				}
				sbQuery.Append(" AND S.����b�c   = SM01.����b�c(+) \n");
				sbQuery.Append(" AND S.����b�c   = SM01.����b�c(+) \n");
				sbQuery.Append(" AND S.�ב��l�b�c = SM01.�ב��l�b�c(+) \n");
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
				sbQuery.Append(" AND S.����b�c   = CM01.����b�c(+) \n");
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END

				OracleDataReader reader;

//				sbQuery2.Append(GET_CSVWRITE3_SELECT);
//				sbQuery2.Append(GET_CSVWRITE3_FROM_1);
//				sbQuery2.Append(sbQuery);
				sbQuery2.Append("SELECT * FROM ( \n");
				sbQuery2.Append(GET_CSVWRITE3_SELECT).Replace("S.","ST01.");
				sbQuery2.Append(GET_CSVWRITE3_FROM_1);
				sbQuery2.Append(sbQuery).Replace("S.","ST01.");
				sbQuery2.Append(" UNION \n");
				sbQuery2.Append(GET_CSVWRITE3_SELECT).Replace("S.","ST02.");
				sbQuery2.Append(GET_CSVWRITE3_FROM_2);
				sbQuery2.Append(sbQuery).Replace("S.","ST02.");
				sbQuery2.Append(") S \n");

				if(s���t�敪 == "0"){
					sbQuery2.Append(GET_CSVWRITE3_SORT_1);
				}else{
					sbQuery2.Append(GET_CSVWRITE3_SORT_2);
				}
				reader = CmdSelect(sUser, conn2, sbQuery2);

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read()){
					sbData = new StringBuilder(1024);
					if(s�b�r�u�o�͌`��.Equals("1")){
						sbData.Append(sDbl + sSng + reader.GetString(3).Trim() + sDbl);		// �׎�l�b�c
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(5).Trim() + sDbl);	// �׎�l�d�b�ԍ�
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim() + sDbl);	// �׎�l�Z���P
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(7).Trim() + sDbl);	// �׎�l�Z���Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(8).Trim() + sDbl);	// �׎�l�Z���R
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim() + sDbl);	// �׎�l���O�P
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(10).Trim() + sDbl);	// �׎�l���O�Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// �׎�l�X�֔ԍ�
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// ����v
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// ���X�b�c
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// �ב��l�b�c
						sbData.Append(sKanma + reader.GetString(23).Trim()                     );	// ��
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
//						sbData.Append(sKanma + reader.GetDecimal(36).ToString().Trim()         );	// �ː�
//						sbData.Append(sKanma + reader.GetString(24).Trim()                     );	// �d��
						s�^���ː� = reader.GetString(40).TrimEnd();
						s�^���d�� = reader.GetString(41).TrimEnd();
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
						s�d�ʓ��͐��� = reader.GetString(42).TrimEnd();
						if(s�d�ʓ��͐��� == "1"
						&& s�^���ː�.Length == 0 && s�^���d��.Length == 0
//						&& (reader.GetString(24).TrimEnd() != "0" || reader.GetDecimal(36) != 0)
						){
							sbData.Append(sKanma + reader.GetDecimal(36).ToString().TrimEnd());	// �ː�
							sbData.Append(sKanma + reader.GetString(24).TrimEnd());				// �d��
						}else{
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
							d�ː� = 0;
							d�d�� = 0;
							if(s�^���ː�.Length > 0){
								try{
									d�ː� = Decimal.Parse(s�^���ː�);
								}catch(Exception){}
							}
							if(s�^���d��.Length > 0){
								try{
									d�d�� = Decimal.Parse(s�^���d��);
								}catch(Exception){}
							}
							sbData.Append(sKanma + d�ː�.ToString());		// �ː�
							sbData.Append(sKanma + d�d��.ToString());		// �d��
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
						}
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// �A���w���P
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// �A���w���Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// �i���L���P
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// �i���L���Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// �i���L���R

						if(reader.GetString(25).Trim() == "0"){
							sbData.Append(sKanma + sDbl + sDbl);										// �w���
						}else{
							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// �w���
						}

						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// ���q�l�o�הԍ�
						sbData.Append(sKanma + sDbl + sDbl);										// �\��
						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// �����敪
						sbData.Append(sKanma + reader.GetString(32).Trim()                     );	// �ی����z
						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl);	// �o�ד�
						sbData.Append(sKanma + sDbl + sDbl);								// �o�^���i�ȗ��j
					}else{
						sbData.Append(sDbl + reader.GetString(0).Trim() + sDbl);					// �o�^��
						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl       );	// �o�ד�
						string sNo = reader.GetString(2).Trim();									// �����ԍ�(XXX-XXXX-XXXX)
						if(sNo.Length == 15){
							sbData.Append(sKanma + sDbl + sNo.Substring(4,3)
								+ "-" + sNo.Substring(7,4) + "-" + sNo.Substring(11) + sDbl);
						}else{
							sbData.Append(sKanma + sDbl + " " + sDbl);
						}
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(3).Trim() + sDbl);	// �׎�l�b�c
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// �׎�l�X�֔ԍ�
						sbData.Append(sKanma + sDbl + reader.GetString(5).Trim() + sDbl);			// �׎�l�d�b�ԍ�
						sbData.Append(sKanma + sDbl + reader.GetString(6).Trim() + sDbl);			// �׎�l�Z���P
						sbData.Append(sKanma + sDbl + reader.GetString(7).Trim() + sDbl);			// �׎�l�Z���Q
						sbData.Append(sKanma + sDbl + reader.GetString(8).Trim() + sDbl);			// �׎�l�Z���R
						sbData.Append(sKanma + sDbl + reader.GetString(9).Trim() + sDbl);			// �׎�l���O�P
						sbData.Append(sKanma + sDbl + reader.GetString(10).Trim() + sDbl);			// �׎�l���O�Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// ����v
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// ���X�b�c
						sbData.Append(sKanma + sDbl + reader.GetString(13).Trim() + sDbl       );	// ���X��
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// �ב��l�b�c
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(15).Trim() + sDbl);	// �ב��l�X�֔ԍ�

						string sTel = reader.GetString(16).Trim();									// �ב��l�d�b�ԍ�
						if(sTel.Length != 0){
							sbData.Append(sKanma + sDbl + "(" + sTel + ")"
								+ "-" + reader.GetString(17).Trim() + "-" + reader.GetString(18).Trim() + sDbl);
						}else{
							sbData.Append(sKanma + sDbl + " " + sDbl);
						}

						sbData.Append(sKanma + sDbl + reader.GetString(19).Trim() + sDbl);			// �ב��l�Z���P
						sbData.Append(sKanma + sDbl + reader.GetString(20).Trim() + sDbl);			// �ב��l�Z���Q
						sbData.Append(sKanma + sDbl + reader.GetString(21).Trim() + sDbl);			// �ב��l���O�P
						sbData.Append(sKanma + sDbl + reader.GetString(22).Trim() + sDbl);			// �ב��l���O�Q
						sbData.Append(sKanma + reader.GetString(23)                            );	// ��
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� START
//						d�ː� = reader.GetDecimal(36);												// �ː�
//						d�ː� = d�ː� * 8;
//						if(d�ː� == 0){
//							sbData.Append(sKanma + reader.GetString(24)                            );	// �d��
//						}else{
//							sbData.Append(sKanma + d�ː�.ToString()                            );
//						}
						s�^���ː� = reader.GetString(40).TrimEnd();
						s�^���d�� = reader.GetString(41).TrimEnd();
						d�ː��d�� = 0;
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
						s�d�ʓ��͐��� = reader.GetString(42).TrimEnd();
						if(s�d�ʓ��͐��� == "1"
						&& s�^���ː�.Length == 0 && s�^���d��.Length == 0
//						&& (reader.GetString(24).TrimEnd() != "0" || reader.GetDecimal(36) != 0)
						){
							d�ː��d�� += (reader.GetDecimal(36) * 8);		// �ː�
							if(reader.GetString(24).TrimEnd().Length > 0){	// �d��
								try{
									d�ː��d�� += Decimal.Parse(reader.GetString(24).TrimEnd());
								}catch(Exception){}
							}
							sbData.Append(sKanma + d�ː��d��.ToString());	// �d��
						}else{
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
							if(s�^���ː�.Length > 0){
								try{
									d�ː��d�� += (Decimal.Parse(s�^���ː�) * 8);
								}catch(Exception){}
							}
							if(s�^���d��.Length > 0){
								try{
									d�ː��d�� += Decimal.Parse(s�^���d��);
								}catch(Exception){}
							}
							sbData.Append(sKanma + d�ː��d��.ToString());		// �d��
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� START
						}
// MOD 2011.05.06 ���s�j���� ���q�l���Ƃɏd�ʓ��͐��� END
// MOD 2011.04.13 ���s�j���� �d�ʓ��͕s�Ή� END
						if(reader.GetString(25).Trim() == "0"){
							sbData.Append(sKanma + sDbl + sDbl);										// �w���
						}else{
							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// �w���
						}
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// �A���w���P
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// �A���w���Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// �i���L���P
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// �i���L���Q
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// �i���L���R
						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// �����敪
						sbData.Append(sKanma + reader.GetString(32)                            );	// �ی����z
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// ���q�l�o�הԍ�
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(34).Trim() + sDbl);	// ���Ӑ�b�c
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(35).Trim() + sDbl);	// ���ۂb�c
					}
// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ� START
					if(s�z���r�o�͌`��.Equals("1"))
					{
						sbData.Append(sKanma + sDbl + reader.GetString(43).Trim() + sDbl       );	// �z�����t�E����
					}
					sList.Add(sbData);
// MOD 2014.03.20 BEVAS�j���� �b�r�u�o�͂ɔz�����t�E������ǉ� END
				}

//				sbQuery2 = new StringBuilder(1024);
//				sbQuery2.Append(GET_CSVWRITE3_SELECT);
//				sbQuery2.Append(GET_CSVWRITE3_FROM_2);
//				sbQuery2.Append(sbQuery);
//				if(s���t�敪 == "0"){
//					sbQuery2.Append(GET_CSVWRITE3_SORT_1);
//				}else{
//					sbQuery2.Append(GET_CSVWRITE3_SORT_2);
//				}
//				reader = CmdSelect(sUser, conn2, sbQuery2);
//
//				while (reader.Read()){
//					sbData = new StringBuilder(1024);
//					if(s�b�r�u�o�͌`��.Equals("1")){
//						sbData.Append(sDbl + sSng + reader.GetString(3).Trim() + sDbl);		// �׎�l�b�c
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(5).Trim() + sDbl);	// �׎�l�d�b�ԍ�
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim() + sDbl);	// �׎�l�Z���P
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(7).Trim() + sDbl);	// �׎�l�Z���Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(8).Trim() + sDbl);	// �׎�l�Z���R
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim() + sDbl);	// �׎�l���O�P
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(10).Trim() + sDbl);	// �׎�l���O�Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// �׎�l�X�֔ԍ�
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// ����v
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// ���X�b�c
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// �ב��l�b�c
//						sbData.Append(sKanma + reader.GetString(23).Trim()                     );	// ��
//						sbData.Append(sKanma + reader.GetDecimal(36).ToString().Trim()         );	// �ː�
//						sbData.Append(sKanma + reader.GetString(24).Trim()                     );	// �d��
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// �A���w���P
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// �A���w���Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// �i���L���P
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// �i���L���Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// �i���L���R
//
//						if(reader.GetString(25).Trim() == "0"){
//							sbData.Append(sKanma + sDbl + sDbl);										// �w���
//						}else{
//							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// �w���
//						}
//
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// ���q�l�o�הԍ�
//						sbData.Append(sKanma + sDbl + sDbl);										// �\��
//						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// �����敪
//						sbData.Append(sKanma + reader.GetString(32).Trim()                     );	// �ی����z
//						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl);	// �o�ד�
//						sbData.Append(sKanma + sDbl + sDbl);								// �o�^���i�ȗ��j
//					}else{
//						sbData.Append(sDbl + reader.GetString(0).Trim() + sDbl);					// �o�^��
//						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl       );	// �o�ד�
//						string sNo = reader.GetString(2).Trim();									// �����ԍ�(XXX-XXXX-XXXX)
//						if(sNo.Length == 15){
//							sbData.Append(sKanma + sDbl + sNo.Substring(4,3)
//								+ "-" + sNo.Substring(7,4) + "-" + sNo.Substring(11) + sDbl);
//						}else{
//							sbData.Append(sKanma + sDbl + " " + sDbl);
//						}
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(3).Trim() + sDbl);	// �׎�l�b�c
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// �׎�l�X�֔ԍ�
//						sbData.Append(sKanma + sDbl + reader.GetString(5).Trim() + sDbl);			// �׎�l�d�b�ԍ�
//						sbData.Append(sKanma + sDbl + reader.GetString(6).Trim() + sDbl);			// �׎�l�Z���P
//						sbData.Append(sKanma + sDbl + reader.GetString(7).Trim() + sDbl);			// �׎�l�Z���Q
//						sbData.Append(sKanma + sDbl + reader.GetString(8).Trim() + sDbl);			// �׎�l�Z���R
//						sbData.Append(sKanma + sDbl + reader.GetString(9).Trim() + sDbl);			// �׎�l���O�P
//						sbData.Append(sKanma + sDbl + reader.GetString(10).Trim() + sDbl);			// �׎�l���O�Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// ����v
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// ���X�b�c
//						sbData.Append(sKanma + sDbl + reader.GetString(13).Trim() + sDbl       );	// ���X��
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// �ב��l�b�c
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(15).Trim() + sDbl);	// �ב��l�X�֔ԍ�
//
//						string sTel = reader.GetString(16).Trim();									// �ב��l�d�b�ԍ�
//						if(sTel.Length != 0){
//							sbData.Append(sKanma + sDbl + "(" + sTel + ")"
//								+ "-" + reader.GetString(17).Trim() + "-" + reader.GetString(18).Trim() + sDbl);
//						}else{
//							sbData.Append(sKanma + sDbl + " " + sDbl);
//						}
//
//						sbData.Append(sKanma + sDbl + reader.GetString(19).Trim() + sDbl);			// �ב��l�Z���P
//						sbData.Append(sKanma + sDbl + reader.GetString(20).Trim() + sDbl);			// �ב��l�Z���Q
//						sbData.Append(sKanma + sDbl + reader.GetString(21).Trim() + sDbl);			// �ב��l���O�P
//						sbData.Append(sKanma + sDbl + reader.GetString(22).Trim() + sDbl);			// �ב��l���O�Q
//						sbData.Append(sKanma + reader.GetString(23)                            );	// ��
//
//						d�ː� = reader.GetDecimal(36);												// �ː�
//						d�ː� = d�ː� * 8;
//						if(d�ː� == 0){
//							sbData.Append(sKanma + reader.GetString(24)                            );	// �d��
//						}else{
//							sbData.Append(sKanma + d�ː�.ToString()                            );
//						}
//						if(reader.GetString(25).Trim() == "0"){
//							sbData.Append(sKanma + sDbl + sDbl);										// �w���
//						}else{
//							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// �w���
//						}
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// �A���w���P
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// �A���w���Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// �i���L���P
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// �i���L���Q
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// �i���L���R
//						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// �����敪
//						sbData.Append(sKanma + reader.GetString(32)                            );	// �ی����z
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// ���q�l�o�הԍ�
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(34).Trim() + sDbl);	// ���Ӑ�b�c
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(35).Trim() + sDbl);	// ���ۂb�c
//					}
//					sList.Add(sbData);
//				}

				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0){
					sRet[0] = "�Y���f�[�^������܂���";
				}else{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext()){
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2010.04.30 ���s�j���� �b�r�u�o�͋@�\�̒ǉ� END
// MOD 2010.10.12 ���s�j���� �^���G���[�Ή� START
		/*********************************************************************
		 * �^���G���[�����m�F
		 * �����F�X���b�c�A�o�ד��J�n�A�o�ד��I���A����
		 * �ߒl�F�X�e�[�^�X�A�o�ד��A�����A...
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Get_UntinErrCntChk(string[] sUser, string[] sData)
		{
			logWriter(sUser, INF, "�^���G���[�����m�F�J�n");
			string s���X�b�c   = sData[0];
			string s�o�ד��J�n = (sData.Length > 1) ? sData[1] : "";
			string s�o�ד��I�� = (sData.Length > 2) ? sData[2] : "";
			string s����       = (sData.Length > 3) ? sData[3] : "5";
			int i���� = 5;
			try{
				i���� = int.Parse(s����);
			}catch(Exception){
			}
			OracleConnection conn2 = null;
			string[] sRet = new string[1+(i����*2)];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			try{
				StringBuilder sbQuery = new StringBuilder(1024);
				OracleDataReader reader;
				sbQuery.Append("SELECT �o�ד�, COUNT(ROWID)  \n");
				sbQuery.Append(" FROM \"�r�s�O�P�o�׃W���[�i��\" \n");
				sbQuery.Append(" WHERE ���X�b�c = '" + s���X�b�c + "' \n");
				if(s�o�ד��J�n.Length > 0){
					sbQuery.Append(" AND �o�ד� >= '" + s�o�ד��J�n + "' \n");
				}
				if(s�o�ד��I��.Length > 0){
					sbQuery.Append(" AND �o�ד� <= '" + s�o�ד��I�� + "' \n");
				}
				sbQuery.Append(" AND ( �^�� > 0 OR ���p > 0 OR ������ > 0 ) \n");
				sbQuery.Append(" AND \"�^���G���[�m�F�e�f\" = ' ' \n");
				sbQuery.Append(" AND �폜�e�f = '1' \n");
				sbQuery.Append(" GROUP BY �o�ד� \n");
				sbQuery.Append(" ORDER BY �o�ד� DESC \n");

				reader = CmdSelect(sUser, conn2, sbQuery);
				int iPos = 1;
				while(reader.Read() && iPos < sRet.Length){
					sRet[iPos  ] = reader.GetString(0).TrimEnd();	// �o�ד�
					sRet[iPos+1] = reader.GetDecimal(1).ToString(); // ����
					iPos+=2;
				}
				disposeReader(reader);
				reader = null;
				if(sRet[1] == null){
					sRet[0] = "�Y���Ȃ�";
				}else{
					sRet[0] = "����I��";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
		/*********************************************************************
		 * �^���G���[�m�F�e�f�X�V
		 * �����F����b�c�A����b�c�A�o�^���A�W���[�i���m�n�A�^���G���[�m�F�e�f
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_UntinErrKakuninFG(string[] sUser, string[] sData)
		{
			logWriter(sUser, INF, "�^���G���[�m�F�e�f�X�V�J�n");
			string s����b�c   = sData[0];
			string s����b�c   = sData[1];
			string s�o�^��     = sData[2];
			string s�W���m�n   = sData[3];
			string s�m�F�e�f   = sData[4];

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();
			try{
				StringBuilder sbQuery = new StringBuilder(1024);
				sbQuery.Append("UPDATE \"�r�s�O�P�o�׃W���[�i��\" \n");
				sbQuery.Append(" SET \"�^���G���[�m�F�e�f\" = '" + s�m�F�e�f + "' ");
				sbQuery.Append(" WHERE ����b�c = '" + s����b�c + "' \n");
				sbQuery.Append(" AND ����b�c = '" + s����b�c + "' \n");
				sbQuery.Append(" AND �o�^�� = '" + s�o�^�� + "' \n");
				sbQuery.Append(" AND \"�W���[�i���m�n\" = " + s�W���m�n + " \n");

				if (CmdUpdate(sUser, conn2, sbQuery) != 0){
					tran.Commit();
					sRet[0] = "����I��";
				}else{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2010.10.12 ���s�j���� �^���G���[�Ή� END
// MOD 2010.11.19 ���s�j���� �z�����Ȃǂ̎擾 START
		/*********************************************************************
		 * �o�׏�񌟍�
		 * �����F[����b�c, ����b�c, �o�^��, �W���[�i���m�n, �����ԍ�]�̔z��
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_SyukkaEtc(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�o�ד��b�r�u�o�͗p�擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet            = new string[sKey.Length + 1];
			string[] s����b�c       = new string[sKey.Length];
			string[] s����b�c       = new string[sKey.Length];
			string[] s�o�^��         = new string[sKey.Length];
			string[] s�W���[�i���m�n = new string[sKey.Length];
			string[] s�����ԍ�     = new string[sKey.Length];

			int i���� = 0;
			string[] sData;
			while(i���� < sKey.Length){
				sData = sKey[i����].Split(',');
				if(sData.Length < 5) break;
				s����b�c      [i����] = sData[0];
				s����b�c      [i����] = sData[1];
				s�o�^��        [i����] = sData[2];
				s�W���[�i���m�n[i����] = sData[3];
				s�����ԍ�    [i����] = sData[4];
				sData = null;
				i����++;
			}
			if(i���� == 0){
				sRet[0] = "�����G���[";
				return sRet;
			}

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try{
				OracleDataReader reader;
				OracleDataReader reader2;
				int iRow = 0;
				for(int iCnt = 0; iCnt < i����; iCnt++){
					iRow++;
					sRet[iRow] = "";
					sRet[iRow] += "�o�׃f�[�^";
					cmdQuery
						= "SELECT * FROM \"�r�s�O�P�o�׃W���[�i��\" \n"
						+ " WHERE ( \n"
						+ " ����b�c = '" + s����b�c[iCnt] + "' \n"
						+ " AND ����b�c = '" + s����b�c[iCnt] + "' \n"
						+ " AND �o�^�� = '" + s�o�^��[iCnt] + "' \n"
						+ " AND \"�W���[�i���m�n\" = " + s�W���[�i���m�n[iCnt] + " \n"
						+ " ) \n"
						;

					reader = CmdSelect(sUser, conn2, cmdQuery);
					if(reader.Read()){
						for(int iCntCol = 0; iCntCol < reader.FieldCount; iCntCol++){
							if(reader.GetValue(iCntCol) is System.String){
								sRet[iRow] += "," + reader.GetString(iCntCol).TrimEnd();
							}else{
								sRet[iRow] += "," + reader.GetDecimal(iCntCol).ToString();
							}
						}
					}else{
						for(int iCntCol = 0; iCntCol < reader.FieldCount; iCntCol++){
							sRet[iRow] += ",";
						}
					}
					disposeReader(reader);
					reader = null;
					if(s�����ԍ�[iCnt].Length > 0){
						sRet[iRow] += ",�z�����";
						cmdQuery
							= "SELECT * FROM �f�s�O�Q�z�� \n"
							+ " WHERE ���[�ԍ� = '0000" + s�����ԍ�[iCnt] + "' \n"
							;
						reader2 = CmdSelect(sUser, conn2, cmdQuery);
						if(reader2.Read()){
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								if(reader2.GetValue(iCntCol) is System.String){
									sRet[iRow] += "," + reader2.GetString(iCntCol).TrimEnd();
								}else{
									sRet[iRow] += "," + reader2.GetDecimal(iCntCol).ToString();
								}
							}
						}else{
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								sRet[iRow] += ",";
							}
						}
						disposeReader(reader2);
						reader2 = null;

						sRet[iRow] += ",�^�����";
						cmdQuery
							= "SELECT * FROM �f�s�O�R���[�^�� \n"
							+ " WHERE ���[�ԍ� = '0000" + s�����ԍ�[iCnt] + "' \n"
							;
						reader2 = CmdSelect(sUser, conn2, cmdQuery);
						if(reader2.Read()){
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								if(reader2.GetValue(iCntCol) is System.String){
									sRet[iRow] += "," + reader2.GetString(iCntCol).TrimEnd();
								}else{
									sRet[iRow] += "," + reader2.GetDecimal(iCntCol).ToString();
								}
							}
						}else{
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								sRet[iRow] += ",";
							}
						}
						disposeReader(reader2);
						reader2 = null;
					}
				}
				if(iRow == 0){
					sRet[0] = "�Y���f�[�^������܂���";
				}else{
					sRet[0] = "����I��";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2010.11.19 ���s�j���� �z�����Ȃǂ̎擾 END
// MOD 2011.02.02 ���s�j���� �o�׃f�[�^�o�ד��͈͎擾 START
		/*********************************************************************
		 * �o�׃f�[�^�o�ד��͈͎擾
		 * �����F[����b�c, ����b�c, �o�^��, �W���[�i���m�n, �����ԍ�]�̔z��
		 * �ߒl�F�X�e�[�^�X�A�X�V����
		 *
		 *********************************************************************/
		private static string GET_SYUKKABIMINMAX_SELECT1
			= "SELECT MIN(�o�ד�), MAX(�o�ד�) \n"
			+ " FROM \"�r�s�O�P�o�׃W���[�i��\" \n"
			+ " WHERE �폜�e�f = '0' \n"
			;
		private static string GET_SYUKKABIMINMAX_SELECT2
			= "SELECT MIN(�o�ד�), MAX(�o�ד�) \n"
			+ " FROM \"�r�s�O�Q�o�ח���\" \n"
			+ " WHERE �폜�e�f = '0' \n"
			;
		[WebMethod]
		public string[] Get_SyukkaBiMinMax(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�o�׃f�[�^�o�ד��͈͎擾�J�n");

			string s����b�c = (sKey.Length > 0) ? sKey[0] : "";
			string s����b�c = (sKey.Length > 1) ? sKey[1] : "";

			OracleConnection conn2 = null;
			string[] sRet = new string[]{"","","","",""};

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			try{
				OracleDataReader reader;
				sbQuery.Append(GET_SYUKKABIMINMAX_SELECT1);
				if(s����b�c.Length > 0){
					sbQuery.Append(" AND ����b�c = '" + s����b�c + "' \n");
				}
				if(s����b�c.Length > 0){
					sbQuery.Append(" AND ����b�c = '" + s����b�c + "' \n");
				}
				reader = CmdSelect(sUser, conn2, sbQuery);
				if(reader.Read()){
					sRet[1] = reader.GetString(0).TrimEnd();
					sRet[2] = reader.GetString(1).TrimEnd();
				}
				disposeReader(reader);
				reader = null;

				sbQuery = new StringBuilder(1024);
				sbQuery.Append(GET_SYUKKABIMINMAX_SELECT2);
				if(s����b�c.Length > 0){
					sbQuery.Append(" AND ����b�c = '" + s����b�c + "' \n");
				}
				if(s����b�c.Length > 0){
					sbQuery.Append(" AND ����b�c = '" + s����b�c + "' \n");
				}
				reader = CmdSelect(sUser, conn2, sbQuery);
				if(reader.Read()){
					sRet[3] = reader.GetString(0).TrimEnd();
					sRet[4] = reader.GetString(1).TrimEnd();
				}
				disposeReader(reader);
				reader = null;

				if(sRet[1].Length == 0){
					sRet[0] = "�Y���o�׃f�[�^������܂���";
				}else if(sRet[3].Length == 0){
					sRet[0] = "�Y���o�ח����f�[�^������܂���";
				}else{
					sRet[0] = "����I��";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2011.02.02 ���s�j���� �o�׃f�[�^�o�ד��͈͎擾 END

// ADD 2015.11.24 bevas�j���{ �o�׎��ѕ\����@�\�ǉ�(is-2�Ǘ�) START
		/*********************************************************************
		 * �o�׎��ш���f�[�^�擾
		 * �����F����b�c�A����b�c�A�o�ד� or �o�^���A
		 *		 �J�n���A�I����
		 * �ߒl�F�X�e�[�^�X�A�o�^���A�׎�l�b�c...
		 *
		 *********************************************************************/
		private static string GET_SYUKKA_SELECT_4
			= "SELECT \n"
			+	"  S.�o�^�� \n"
			+	", S.�o�ד� \n"
			+	", S.�����ԍ� \n"
			+	", S.�׎�l�b�c \n"
			+	", S.�X�֔ԍ� \n"
			+	", S.�d�b�ԍ��P \n"
			+	", S.�d�b�ԍ��Q \n"
			+	", S.�d�b�ԍ��R \n"
			+	", S.�Z���P \n"
			+	", S.�Z���Q \n"
			+	", S.�Z���R \n"
			+	", S.���O�P \n"
			+	", S.���O�Q \n"
			+	", S.���X�b�c \n"
			+	", S.���X�� \n"
			+	", S.�ב��l�b�c \n"
			+	", NVL(SM01.�X�֔ԍ�, ' ') \n"
			+	", NVL(SM01.�d�b�ԍ��P, ' ') \n"
			+	", NVL(SM01.�d�b�ԍ��Q, ' ') \n"
			+	", NVL(SM01.�d�b�ԍ��R, ' ') \n"
			+	", NVL(SM01.�Z���P, ' ') \n"
			+	", NVL(SM01.�Z���Q, ' ') \n"
			+	", NVL(SM01.���O�P, ' ') \n"
			+	", NVL(SM01.���O�Q, ' ') \n"
			+	", S.�ב��l������ \n"
			+	", TO_CHAR(S.��) \n"
			+	", TO_CHAR(S.�d��) \n"
			+	", S.�w��� \n"
			+	", S.�A���w���P \n"
			+	", S.�A���w���Q \n"
			+	", S.�i���L���P \n"
			+	", S.�i���L���Q \n"
			+	", S.�i���L���R \n"
			+	", S.�����敪 \n"
			+	", TO_CHAR(S.�ی����z) \n"
			+	", S.���q�l�o�הԍ� \n"
			+	", TO_CHAR(S.�ː�) \n"
			+	", S.�w����敪 \n"
			+	", TO_CHAR(S.�^�� + S.���p) \n"
			+	", NVL(CM01.�L���A�g�e�f, '0') \n"
			+	", S.���Ӑ�b�c \n"
			+	", S.���ۂb�c \n"
			+	", S.���ۖ� \n"
			+	", S.�^���ː� \n"
			+	", S.�^���d�� \n"
			+	", NVL(CM01.�ۗ�����e�f, '0') \n"
			+	", S.�i���L���S \n"
			+	", S.�i���L���T \n"
			+	", S.�i���L���U \n"
			+	", S.�����O�R \n"
			//is-2�Ǘ����ł̐V�K�ǉ���
			+	", S.����b�c \n"
			+	", S.����b�c \n"
			+	", CM01.����� \n"
			+	", S.\"�W���[�i���m�n\" \n"
			;

		private static string GET_PUBLISHEDPRINT4_FROM_1
			= " FROM \"�r�s�O�P�o�׃W���[�i��\" ST01 , �r�l�O�P�ב��l SM01 \n"
			+ ", �b�l�O�P��� CM01 \n"
			;

		private static string GET_PUBLISHEDPRINT4_FROM_2 
			= " FROM \"�r�s�O�Q�o�ח���\"       ST02 , �r�l�O�P�ב��l SM01 \n"
			+ ", �b�l�O�P��� CM01 \n"
			;

		private static string GET_PUBLISHEDPRINT4_SORT_1
			= " ORDER BY S.�o�ד�, S.�ב��l�b�c, S.����b�c, S.����b�c, S.�o�^��, S.\"�W���[�i���m�n\" "
			;

		private static string GET_PUBLISHEDPRINT4_SORT_2
			= " ORDER BY           S.�ב��l�b�c, S.����b�c, S.����b�c, S.�o�^��, S.\"�W���[�i���m�n\" "
			;

		[WebMethod]
		public ArrayList Get_PublishedPrintData4(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�o�׎��ш���f�[�^�S�J�n");

			string s����b�c   = sKey[0];
			string s����b�c   = sKey[1];
			string s�ב��l�b�c = sKey[2];
			string s���t�敪   = sKey[3];
			string s�J�n��     = sKey[4];
			string s�I����     = sKey[5];
			string s��Ԃb�c   = sKey[6];
			bool   b���o��     = sKey[6].Equals("90");
			string s�����ԍ� = sKey[7];
			string s�폜�e�f   = sKey[8];
			string s���X�b�c   = sKey[9];

			OracleConnection conn2 = null;
			ArrayList alRet = new ArrayList();

			string[] sRet = new string[1];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				alRet.Add(sRet);
				return alRet;
			}

			string  s�^���ː� = "";
			string  s�^���d�� = "";
			decimal d�ː� = 0;
			decimal d�d�� = 0;
			string  s�d�ʓ��͐��� = "0";

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			try
			{
				if(s�����ԍ�.Length == 0)
				{
					/** (1) ����������[�����ԍ�]�����͂���Ă��Ȃ��ꍇ */
					if(b���o��)
					{
						/** (1-1-1) [�A����]��[���o��]�̏ꍇ */
						sbQuery.Append(", �r�s�O�T���o�ו� ST05 \n");
						// ����b�c
						if(s����b�c.Length > 0)
						{
							sbQuery.Append(" WHERE ST05.����b�c = '" + s����b�c + "' \n");
						}
						else
						{
							sbQuery.Append(" WHERE ST05.����b�c > ' ' \n");
						}

						// ����b�c
						if(s����b�c.Length > 0)
						{
							sbQuery.Append(" AND ST05.����b�c = '" + s����b�c + "' \n");
						}

						// �o�ד��^�o�^��
						if(s���t�敪 == "0")
						{
							// [�o�ד�]��[�o�ד�]�̏ꍇ
							sbQuery.Append(" AND ST05.�o�ד�  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
						}
						else
						{
							// [�o�ד�]��[�o�^��]�̏ꍇ
							sbQuery.Append(" AND ST05.�o�^��  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
						}

						sbQuery.Append(" AND ST05.�o�ד� < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
						sbQuery.Append(" AND ST05.���   = '02' \n");

						// ���X�b�c
						if(s���X�b�c.Length > 0)
						{
							sbQuery.Append(" AND ST05.���X�b�c = '"+ s���X�b�c + "' \n");
						}

						// �e�e�[�u�����m�̌�������
						sbQuery.Append(" AND ST05.����b�c = S.����b�c \n");
						sbQuery.Append(" AND ST05.����b�c = S.����b�c \n");
						sbQuery.Append(" AND ST05.�o�^�� = S.�o�^�� \n");
						sbQuery.Append(" AND ST05.\"�W���[�i���m�n\" = S.\"�W���[�i���m�n\" \n");
					}
					else
					{
						/** (1-1-2) [�A����]��[���o��]�ȊO�̏ꍇ */
						// ����b�c
						if(s����b�c.Length > 0)
						{
							sbQuery.Append(" WHERE S.����b�c = '" + s����b�c + "' \n");
						}
						else
						{
							sbQuery.Append(" WHERE S.����b�c > ' ' \n");
						}

						// ����b�c
						if(s����b�c.Length > 0)
						{
							sbQuery.Append(" AND S.����b�c = '" + s����b�c + "' \n");
						}
					}

					/** (1-2) ���ʌ�������(�����ԍ��̏����Ȃ�) */
					// �ב��l�b�c
					if(s�ב��l�b�c.Length > 0)
					{
						sbQuery.Append(" AND S.�ב��l�b�c = '"+ s�ב��l�b�c + "' \n");
					}

					// �o�ד��^�o�^��
					if(s���t�敪 == "0")
					{
						// [�o�ד�]��[�o�ד�]�̏ꍇ
						sbQuery.Append(" AND S.�o�ד�  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
					}
					else
					{
						// [�o�ד�]��[�o�^��]�̏ꍇ
						sbQuery.Append(" AND S.�o�^��  BETWEEN '"+ s�J�n�� + "' AND '"+ s�I���� +"' \n");
					}

					// ���
					if(s��Ԃb�c == "00")
					{
						// [�A����]��[�����s]�̏ꍇ�A�������Ȃ�
						;
					}
					else
					{
						// [�A����]��[�����s]�ȊO�̏ꍇ
						if(b���o��)
						{
							// [�A����]��[���o��]�̏ꍇ
							sbQuery.Append(" AND S.�o�ד� < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
							sbQuery.Append(" AND S.���   = '02' \n");
						}
						else
						{
							// [�A����]��[���o��]�ȊO�̏ꍇ
							sbQuery.Append(" AND S.��� = '"+ s��Ԃb�c + "' \n");
						}
					}
				}
				else
				{
					/** (2) ����������[�����ԍ�]�����͂���Ă����ꍇ */
					// �����ԍ�
					sbQuery.Append(" WHERE S.�����ԍ� = '"+ s�����ԍ� + "' \n");
				}

				/** (3) ���ʌ������� */
				// �폜�e�f
				if(s�폜�e�f != "0")
				{
					if(s�폜�e�f == "1")
					{
						// [���]��[�폜]�̏ꍇ
						sbQuery.Append(" AND S.�폜�e�f = '1' \n");
					}
					else
					{
						// [���]��[�o�ו�]�̏ꍇ
						sbQuery.Append(" AND S.�폜�e�f = '0' \n");
					}
				}

				// ���X�b�c
				if(s���X�b�c.Length > 0)
				{
					sbQuery.Append(" AND S.���X�b�c = '"+ s���X�b�c + "' \n");
				}

				// �e�e�[�u�����m�̌�������
				sbQuery.Append(" AND S.����b�c   = SM01.����b�c(+) \n");
				sbQuery.Append(" AND S.����b�c   = SM01.����b�c(+) \n");
				sbQuery.Append(" AND S.�ב��l�b�c = SM01.�ב��l�b�c(+) \n");
				sbQuery.Append(" AND S.����b�c   = CM01.����b�c(+) \n");

				OracleDataReader reader;
				sbQuery2.Append("SELECT * FROM ( \n");
				sbQuery2.Append(GET_SYUKKA_SELECT_4).Replace("S.","ST01.");
				sbQuery2.Append(GET_PUBLISHEDPRINT4_FROM_1);
				sbQuery2.Append(sbQuery).Replace("S.","ST01.");
				sbQuery2.Append(" UNION \n");
				sbQuery2.Append(GET_SYUKKA_SELECT_4).Replace("S.","ST02.");
				sbQuery2.Append(GET_PUBLISHEDPRINT4_FROM_2);
				sbQuery2.Append(sbQuery).Replace("S.","ST02.");
				sbQuery2.Append(") S \n");

				// �\�[�g�L�[�̐ݒ�
				if(s���t�敪 == "0")
				{
					// [�o�ד�]��[�o�ד�]�̏ꍇ
					sbQuery2.Append(GET_PUBLISHEDPRINT4_SORT_1);
				}
				else
				{
					// [�o�ד�]��[�o�^��]�̏ꍇ
					sbQuery2.Append(GET_PUBLISHEDPRINT4_SORT_2);
				}

				reader = CmdSelect(sUser, conn2, sbQuery2);
				while (reader.Read())
				{
					string[] sData = new string[50];
					for(int iCnt = 0; iCnt < 39; iCnt++)
					{
						sData[iCnt] = reader.GetString(iCnt);
					}

					// �L���A�g�e�f���u1�v�̏ꍇ�A�^���͕\�����Ȃ�
					if(reader.GetString(39).Equals("1"))
					{
						sData[38] = "0";
					}

					sData[39] = reader.GetString(40);	//���Ӑ�b�c
					sData[40] = reader.GetString(41);	//���ۂb�c
					sData[41] = reader.GetString(42);	//���ۖ�
					sData[42] = reader.GetString(43);   //�^���ː�
					sData[43] = reader.GetString(44);   //�^���d��
					s�^���ː� = reader.GetString(43).TrimEnd();
					s�^���d�� = reader.GetString(44).TrimEnd();
					s�d�ʓ��͐��� = reader.GetString(45).TrimEnd();
					if(s�d�ʓ��͐��� == "1" && s�^���ː�.Length == 0 && s�^���d��.Length == 0)
					{
						sData[42] = sData[36]; //�^���ː� = �ː�
						sData[43] = sData[26]; //�^���d�� = �d��
					}
					else
					{
						d�ː� = 0;
						d�d�� = 0;
						if(s�^���ː�.Length > 0)
						{
							try
							{
								d�ː� = Decimal.Parse(s�^���ː�);
							}
							catch(Exception){}
						}
						if(s�^���d��.Length > 0)
						{
							try
							{
								d�d�� = Decimal.Parse(s�^���d��);
							}
							catch(Exception){}
						}
						sData[26] = d�d��.ToString();	// �d��
						sData[36] = d�ː�.ToString();	// �ː�
					}

					sData[44] = reader.GetString(46).TrimEnd(); // �i���L���S
					sData[45] = reader.GetString(47).TrimEnd(); // �i���L���T
					sData[46] = reader.GetString(48).TrimEnd(); // �i���L���U
					sData[47] = reader.GetString(49).Trim();	// �z�����t�E����
					sData[48] = reader.GetString(50).Trim();	// ����b�c
					sData[49] = reader.GetString(52).Trim();	// �����

					alRet.Add(sData);
				}

				disposeReader(reader);
				reader = null;

				if (alRet.Count == 0)
				{
					// �q�b�g������0���̏ꍇ
					sRet[0] = "�Y���f�[�^������܂���";
					alRet.Add(sRet);
				}
				else if(alRet.Count > 1000)
				{
					// �q�b�g������1000���𒴂���ꍇ
					sRet[0] = "1000���I�[�o�[";
					alRet = new ArrayList();
					alRet.Add(sRet);
				}
				else
				{
					// ����ȊO�i���q�b�g������1000���ȉ��j�̏ꍇ
					sRet[0] = "����I��";
					alRet.Insert(0, sRet);
				}
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
				alRet.Insert(0, sRet);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				alRet.Insert(0, sRet);
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return alRet;
		}

		/*********************************************************************
		 * �o�׈���f�[�^�擾
		 * �����F����b�c�A����b�c�A�o�^���A�W���[�i���m�n
		 * �ߒl�F�X�e�[�^�X�A�׎�l�b�c�A�d�b�ԍ��A�Z��...
		 *
		 *********************************************************************/
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
		private static string GET_INVOICEPRINT_SELECT
			= "SELECT \n"
			+	"  S.�׎�l�b�c \n"
			+	", S.�d�b�ԍ��P \n"
			+	", S.�d�b�ԍ��Q \n"
			+	", S.�d�b�ԍ��R \n"
			+	", S.�Z���P \n"
			+	", S.�Z���Q \n"
			+	", S.�Z���R \n"
			+	", S.���O�P \n"
			+	", S.���O�Q \n"
			+	", S.�o�ד� \n"
			+	", S.�����ԍ� \n"
			+	", S.�X�֔ԍ� \n"
			+	", S.���X�b�c \n"
			+	", NVL(CM14.�X���b�c, S.���X�b�c) \n"
			+	", SM01.�d�b�ԍ��P \n"
			+	", SM01.�d�b�ԍ��Q \n"
			+	", SM01.�d�b�ԍ��R \n"
			+	", SM01.�Z���P \n"
			+	", SM01.�Z���Q \n"
			+	", SM01.�Z���R \n"
			+	", SM01.���O�P \n"
			+	", SM01.���O�Q \n"
			+	", S.�� \n"
			+	", S.�d�� \n"
			+	", S.�ی����z \n"
			+	", S.�w��� \n"
			+	", S.�A���w���P \n"
			+	", S.�A���w���Q \n"
			+	", S.�i���L���P \n"
			+	", S.�i���L���Q \n"
			+	", S.�i���L���R \n"
			+	", S.�����敪 \n"
			+	", S.����󔭍s�ςe�f \n"
			+	", S.�ː� \n"
			+	", S.�ב��l������ \n"
			+	", S.���q�l�o�הԍ� \n"
			+	", S.�A���w���b�c�P \n"
			+	", S.�A���w���b�c�Q \n"
			+	", S.�w����敪 \n"
			+	", S.�X�֔ԍ� \n"
			+	", S.�d���b�c \n"
			+	", NVL(CM10.�X����, S.���X��) \n"
			+	", S.�o�׍ςe�f \n"
			+	", SM01.�X�֔ԍ� \n"
			+	", NVL(CM01.�ۗ�����e�f, '0') \n"
			+	", S.�i���L���S \n"
			+	", S.�i���L���T \n"
			+	", S.�i���L���U \n"
			+	", S.���X�� \n"
			+	", S.���X�b�c \n"
			+	", S.���X�� \n";

		private static string GET_INVOICEPRINT_FROM_1
			= " FROM \"�r�s�O�P�o�׃W���[�i��\" ST01 \n"
			+ " LEFT JOIN �b�l�O�Q���� CM02 \n"
			+ " ON  S.����b�c = CM02.����b�c \n"
			+ " AND S.����b�c = CM02.����b�c \n"
			+ " LEFT JOIN �b�l�P�S�X�֔ԍ� CM14 \n"
			+ " ON CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
			+ " LEFT JOIN �b�l�P�O�X�� CM10 \n"
			+ " ON CM14.�X���b�c = CM10.�X���b�c \n"
			+ " LEFT JOIN �b�l�O�P��� CM01 \n"
			+ " ON S.����b�c = CM01.����b�c\n"
			+ ", \"�r�l�O�P�ב��l\" SM01 \n"
			;

		private static string GET_INVOICEPRINT_FROM_2 
			= " FROM \"�r�s�O�Q�o�ח���\" ST02 \n"
			+ " LEFT JOIN �b�l�O�Q���� CM02 \n"
			+ " ON  S.����b�c = CM02.����b�c \n"
			+ " AND S.����b�c = CM02.����b�c \n"
			+ " LEFT JOIN �b�l�P�S�X�֔ԍ� CM14 \n"
			+ " ON CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n"
			+ " LEFT JOIN �b�l�P�O�X�� CM10 \n"
			+ " ON CM14.�X���b�c = CM10.�X���b�c \n"
			+ " LEFT JOIN �b�l�O�P��� CM01 \n"
			+ " ON S.����b�c = CM01.����b�c\n"
			+ ", \"�r�l�O�P�ב��l\" SM01 \n"
			;
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END

		[WebMethod]
		public String[] Get_InvoicePrintData(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�o�׈���f�[�^�擾�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[46];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			decimal d�ː� = 0;
			string s�X�֔ԍ� = "";
			StringBuilder sbQuery  = new StringBuilder(1024);
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
			StringBuilder sbQueryWhere = new StringBuilder(1024);
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
			try
			{
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
//				sbQuery.Append("SELECT ");
//				sbQuery.Append(" ST01.�׎�l�b�c ");
//				sbQuery.Append(",ST01.�d�b�ԍ��P ");
//				sbQuery.Append(",ST01.�d�b�ԍ��Q ");
//				sbQuery.Append(",ST01.�d�b�ԍ��R ");
//				sbQuery.Append(",ST01.�Z���P ");
//				sbQuery.Append(",ST01.�Z���Q ");
//				sbQuery.Append(",ST01.�Z���R ");
//				sbQuery.Append(",ST01.���O�P ");
//				sbQuery.Append(",ST01.���O�Q ");
//				sbQuery.Append(",ST01.�o�ד� ");
//				sbQuery.Append(",ST01.�����ԍ� ");
//				sbQuery.Append(",ST01.�X�֔ԍ� ");
//				sbQuery.Append(",ST01.���X�b�c ");
//				sbQuery.Append(",NVL(CM14.�X���b�c, ST01.���X�b�c) ");
//				sbQuery.Append(",SM01.�d�b�ԍ��P ");
//				sbQuery.Append(",SM01.�d�b�ԍ��Q ");
//				sbQuery.Append(",SM01.�d�b�ԍ��R ");
//				sbQuery.Append(",SM01.�Z���P ");
//				sbQuery.Append(",SM01.�Z���Q ");
//				sbQuery.Append(",SM01.�Z���R ");
//				sbQuery.Append(",SM01.���O�P ");
//				sbQuery.Append(",SM01.���O�Q ");
//				sbQuery.Append(",ST01.�� ");
//				sbQuery.Append(",ST01.�d�� ");
//				sbQuery.Append(",ST01.�ی����z ");
//				sbQuery.Append(",ST01.�w��� ");
//				sbQuery.Append(",ST01.�A���w���P ");
//				sbQuery.Append(",ST01.�A���w���Q ");
//				sbQuery.Append(",ST01.�i���L���P ");
//				sbQuery.Append(",ST01.�i���L���Q ");
//				sbQuery.Append(",ST01.�i���L���R ");
//				sbQuery.Append(",ST01.�����敪 ");
//				sbQuery.Append(",ST01.����󔭍s�ςe�f ");
//				sbQuery.Append(",ST01.�ː� \n");
//				sbQuery.Append(",ST01.�ב��l������ ");
//				sbQuery.Append(",ST01.���q�l�o�הԍ� ");
//				sbQuery.Append(",ST01.�A���w���b�c�P ");
//				sbQuery.Append(",ST01.�A���w���b�c�Q ");
//				sbQuery.Append(",ST01.�w����敪 ");
//				sbQuery.Append(",ST01.�X�֔ԍ� ");
//				sbQuery.Append(",ST01.�d���b�c ");
//				sbQuery.Append(",NVL(CM10.�X����, ST01.���X��) ");
//				sbQuery.Append(",ST01.�o�׍ςe�f ");
//				sbQuery.Append(",SM01.�X�֔ԍ� ");
//				sbQuery.Append(",NVL(CM01.�ۗ�����e�f, '0') \n");
//				sbQuery.Append(",ST01.�i���L���S \n");
//				sbQuery.Append(",ST01.�i���L���T \n");
//				sbQuery.Append(",ST01.�i���L���U \n");
//				sbQuery.Append(",ST01.���X�� ");
//				sbQuery.Append(" FROM \"�r�s�O�P�o�׃W���[�i��\" ST01 \n");
//				sbQuery.Append(" LEFT JOIN �b�l�O�Q���� CM02 \n");
//				sbQuery.Append(" ON ST01.����b�c = CM02.����b�c \n");
//				sbQuery.Append("AND ST01.����b�c = CM02.����b�c \n");
//				sbQuery.Append(" LEFT JOIN �b�l�P�S�X�֔ԍ� CM14 \n");
//				sbQuery.Append(" ON CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n");
//				sbQuery.Append(" LEFT JOIN �b�l�P�O�X�� CM10 \n");
//				sbQuery.Append(" ON CM14.�X���b�c = CM10.�X���b�c \n");
//				sbQuery.Append(" LEFT JOIN �b�l�O�P��� CM01 \n");
//				sbQuery.Append(" ON ST01.����b�c = CM01.����b�c \n");
//				sbQuery.Append(", \"�r�l�O�P�ב��l\" SM01 \n");
//				sbQuery.Append(" WHERE ST01.����b�c = '" + sKey[0] + "' \n");
//				sbQuery.Append(" AND ST01.����b�c = '" + sKey[1] + "' \n");
//				sbQuery.Append(" AND ST01.�o�^�� = '" + sKey[2] + "' \n");
//				sbQuery.Append(" AND ST01.�W���[�i���m�n = '" + sKey[3] + "' \n");
//				sbQuery.Append(" AND ST01.����b�c = SM01.����b�c \n");
//				sbQuery.Append(" AND ST01.����b�c = SM01.����b�c \n");
//				sbQuery.Append(" AND ST01.�ב��l�b�c = SM01.�ב��l�b�c \n");
//				sbQuery.Append(" AND ST01.�폜�e�f = '0' \n");
//				sbQuery.Append(" AND SM01.�폜�e�f = '0' \n");

				//�擾�����̐ݒ�
				sbQueryWhere.Append(" WHERE S.����b�c = '" + sKey[0] + "' \n");
				sbQueryWhere.Append(" AND S.����b�c = '" + sKey[1] + "' \n");
				sbQueryWhere.Append(" AND S.�o�^�� = '" + sKey[2] + "' \n");
				sbQueryWhere.Append(" AND S.\"�W���[�i���m�n\" = '" + sKey[3] + "' \n");
				sbQueryWhere.Append(" AND S.����b�c = SM01.����b�c \n");
				sbQueryWhere.Append(" AND S.����b�c = SM01.����b�c \n");
				sbQueryWhere.Append(" AND S.�ב��l�b�c = SM01.�ב��l�b�c \n");
				sbQueryWhere.Append(" AND S.�폜�e�f = '0' \n");
				sbQueryWhere.Append(" AND SM01.�폜�e�f = '0' \n");

				//SELECT�����\�z
				sbQuery.Append(GET_INVOICEPRINT_SELECT).Replace("S.", "ST01.");  //SELECT��(ST01)
				sbQuery.Append(GET_INVOICEPRINT_FROM_1);                         //FROM��(ST01)
				sbQuery.Append(sbQueryWhere).Replace("S.", "ST01.");             //WHERE��(ST01)
				sbQuery.Append(" UNION \n");
				sbQuery.Append(GET_INVOICEPRINT_SELECT).Replace("S.", "ST02.");  //SELECT��(ST02)
				sbQuery.Append(GET_INVOICEPRINT_FROM_2);                         //FROM��(ST02)
				sbQuery.Append(sbQueryWhere).Replace("S.", "ST02.");             //WHERE��(ST02)
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);
				int iCnt = 0;
				if (reader.Read())
				{
					string s�A�����i�b�c�P = reader.GetString(36).Trim();
					string s�A�����i�b�c�Q = reader.GetString(37).Trim();
					sRet[1]  = reader.GetString(0).Trim();
					sRet[2]  = reader.GetString(1).Trim();
					sRet[3]  = reader.GetString(2).Trim();
					sRet[4]  = reader.GetString(3).Trim();
					sRet[5]  = reader.GetString(4).TrimEnd(); // �׎�l�Z���P
					sRet[6]  = reader.GetString(5).TrimEnd(); // �׎�l�Z���Q
					sRet[7]  = reader.GetString(6).TrimEnd(); // �׎�l�Z���R
					sRet[8]  = reader.GetString(7).TrimEnd(); // �׎�l���O�P
					sRet[9]  = reader.GetString(8).TrimEnd(); // �׎�l���O�Q
					sRet[10] = reader.GetString(9).Trim();
					sRet[11] = reader.GetString(10).Trim();
					sRet[12] = reader.GetString(11).Trim();
					sRet[13] = reader.GetString(12).Trim().PadLeft(4, '0');
					sRet[14] = reader.GetString(13).Trim().PadLeft(4, '0');
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
					//�Г��`�̏ꍇ�́A�o�׃e�[�u���̕��𐳂Ƃ���
					if(sKey[0].Substring(0,2) == "FK")
					{
						sRet[14] = reader.GetString(49).Trim().PadLeft(4, '0');
					}
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
					sRet[15] = reader.GetString(14).Trim();
					sRet[16] = reader.GetString(15).Trim();
					sRet[17] = reader.GetString(16).Trim();
					sRet[18] = reader.GetString(17).TrimEnd(); // �ב��l�Z���P
					sRet[19] = reader.GetString(18).TrimEnd(); // �ב��l�Z���Q
					sRet[20] = reader.GetString(19).TrimEnd(); // �ב��l�Z���R
					sRet[21] = reader.GetString(20).TrimEnd(); // �ב��l���O�P
					sRet[22] = reader.GetString(21).TrimEnd(); // �ב��l���O�Q
					sRet[23] = reader.GetDecimal(22).ToString().Trim();

					if(reader.GetString(44) == "1")
					{
						d�ː� = reader.GetDecimal(33) * 8;
						if(d�ː� == 0)
						{
							sRet[24] = reader.GetDecimal(23).ToString().TrimEnd();
						}
						else
						{
							sRet[24] = d�ː�.ToString().TrimEnd();
						}
					}
					else
					{
						sRet[24] = "";
					}

					sRet[25] = reader.GetDecimal(24).ToString().Trim();
					sRet[26] = reader.GetString(25).Trim();

					// ���Ԏw��̏ꍇ�A�Q�s�ڂ̂ݕ\��
					if (s�A�����i�b�c�P.Equals("100"))
					{
						sRet[27] = reader.GetString(27).TrimEnd();
						sRet[28] = "";
					}
					// �P�s�ڂƂQ�s�ڂ������R�[�h�̏ꍇ�A�Q�s�ڂ�\�����Ȃ�
					else if (s�A�����i�b�c�P.Equals(s�A�����i�b�c�Q))
					{
						sRet[27] = reader.GetString(26).TrimEnd();
						sRet[28] = "";
					}
					else
					{
						sRet[27] = reader.GetString(26).TrimEnd();
						sRet[28] = reader.GetString(27).TrimEnd();
					}

					sRet[29] = reader.GetString(28).TrimEnd(); // �i���L���P
					sRet[30] = reader.GetString(29).TrimEnd(); // �i���L���Q
					sRet[31] = reader.GetString(30).TrimEnd(); // �i���L���R

					// �p�[�Z���̏ꍇ�A"11"
					if (s�A�����i�b�c�P.Equals("001") || s�A�����i�b�c�P.Equals("002"))
					{
						sRet[32] = reader.GetString(31).Trim() + "1";
					}
					else
					{
						sRet[32] = reader.GetString(31).Trim() + "0";
					}

					sRet[33] = reader.GetString(32).Trim(); // ����󔭍s�ςe�f
					sRet[34] = reader.GetString(34).TrimEnd(); // �S���ҁi�����j
					sRet[35] = reader.GetString(35).Trim(); // ���q�l�ԍ�
					sRet[36] = reader.GetString(38).Trim();
					s�X�֔ԍ� = reader.GetString(39).Trim();
					sRet[37] = reader.GetString(40).Trim();		//�d���b�c
					sRet[38] = reader.GetString(41).Trim();		//���X��
// MOD 2016.04.08 bevas) ���{ �Г��`�[�Ή� START
					//�Г��`�̏ꍇ�́A�o�׃e�[�u���̕��𐳂Ƃ���
					if(sKey[0].Substring(0,2) == "FK")
					{
						sRet[38] = reader.GetString(50).Trim();
					}
// MOD 2016.04.08 bevas) ���{ �Г��`�[�Ή� END
					sRet[39] = reader.GetString(42).Trim();		//�o�׍ςe�f
					sRet[40] = reader.GetString(43).Trim();		//���˗���X�֔ԍ�
					sRet[41] = reader.GetString(44).TrimEnd();
					sRet[42] = reader.GetString(45).TrimEnd(); // �i���L���S
					sRet[43] = reader.GetString(46).TrimEnd(); // �i���L���T
					sRet[44] = reader.GetString(47).TrimEnd(); // �i���L���U
					sRet[45] = reader.GetString(48).TrimEnd(); // ���X��
					iCnt++;
				}

				disposeReader(reader);
				reader = null;

				if (iCnt == 0)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					logWriter(sUser, INF, "�o�׈���f�[�^�擾"
						+"["+sKey[1]+"]["+sKey[2]+"]["+sKey[3]+"]:["+sRet[11]+"]"
						+"����󔭍s��["+sRet[33]+"]�o�׍�["+sRet[39]+"]"
						);
				}
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}

		/*********************************************************************
		 * ���X�擾
		 * �����F����b�c�A����b�c
		 * �ߒl�F�X�e�[�^�X�A�X���b�c
		 *
		 *********************************************************************/
		private static string GET_HATUTEN3_SELECT
			= "SELECT CM14.�X���b�c \n"
			+  " FROM �b�l�O�Q���� CM02 \n"
			+      ", �b�l�P�S�X�֔ԍ� CM14 \n"
			;

		[WebMethod]
		public String[] Get_hatuten3(string[] sUser, string sKcode, string sBcode)
		{
			logWriter(sUser, INF, "���X�擾�R�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[2]{"",""};

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
				sbQuery.Append(GET_HATUTEN3_SELECT);
				sbQuery.Append(" WHERE CM02.����b�c = '" + sKcode + "' \n");
				sbQuery.Append(" AND CM02.����b�c = '" + sBcode + "' \n");
				sbQuery.Append(" AND CM02.�X�֔ԍ� = CM14.�X�֔ԍ� \n");

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				if(reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();

					sRet[0] = "����I��";
				}
				else
				{
					sRet[0] = "���p�҂̏W�דX�擾�Ɏ��s���܂���";
				}

				disposeReader(reader);
				reader = null;

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2015.11.24 bevas�j���{ �o�׎��ѕ\����@�\�ǉ�(is-2�Ǘ�) END
// MOD 2015.12.15 bevas) ���{ �A���֎~�G���A�@�\�Ή�(is-2�Ǘ��F���x���C���[�W�����) START
		/*********************************************************************
		 * �z�B�s�\�G���A�`�F�b�N
		 * �@�@���͂��ꂽ�X�֔ԍ�����A
		 * �@�@�b�l�Q�P�z�B�s�\�̑��݃`�F�b�N�����{����B
		 * �����F�X�֔ԍ�
		 * �ߒl�F�X�e�[�^�X�A���������A���b�Z�[�W�A�\���J�n���A�\���I����
		 *********************************************************************/
		[WebMethod]
		public ArrayList Check_NonDeliveryArea(string[] sUser, string sYubinNo)
		{
			logWriter(sUser, INF, "�z�B�s�\�G���A�`�F�b�N�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];  //��ɃX�e�[�^�X���i�[
			ArrayList alRet = new ArrayList();  //�߂�l
			string cmdQuery;  // SQL��
			OracleDataReader reader;
			OracleParameter[] wk_opOraParam	= null;

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				alRet.Add(sRet);
				return alRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				// SQL��
				cmdQuery
					= "SELECT ��������, ���b�Z�[�W, �\���J�n��, �\���I���� \n"
					+ "  FROM �b�l�Q�P�z�B�s�\ \n"
					+ " WHERE �X�֔ԍ� = :p_YubinNo \n"
					+ "   AND �폜�e�f = '0' \n"
					+ " ORDER BY �������� DESC, ���b�Z�[�W DESC \n";
				wk_opOraParam = new OracleParameter[1];
				wk_opOraParam[0] = new OracleParameter("p_YubinNo", OracleDbType.Char, sYubinNo, ParameterDirection.Input); // �X�֔ԍ�

				reader = CmdSelect(sUser, conn2, cmdQuery, wk_opOraParam);
				wk_opOraParam = null;

				// �f�[�^�擾
				while(reader.Read())
				{
					string[] sData = new string[4];
					sData[0] = reader.GetString(0).Trim(); // ��������
					sData[1] = reader.GetString(1).Trim(); // ���b�Z�[�W
					sData[2] = reader.GetString(2).Trim(); // �\���J�n��
					sData[3] = reader.GetString(3).Trim(); // �\���I����

					alRet.Add(sData); //���X�g�Ɋi�[
				}

				disposeReader(reader);
				reader = null;

				if(alRet.Count == 0)
				{
					//�Y���f�[�^�Ȃ�
					sRet[0] = "�Y���f�[�^�Ȃ�";
					alRet.Add(sRet);
				}
				else
				{
					//�Y���f�[�^����
					sRet[0] = "�Y���f�[�^����";
					alRet.Insert(0, sRet);
				}
			}
			catch (OracleException ex)
			{
				// Oracle �̃G���[
				sRet[0] = chgDBErrMsg(sUser, ex);
				alRet.Insert(0, sRet);
			}
			catch (Exception ex)
			{
				// ����ȊO�̃G���[
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				alRet.Insert(0, sRet);
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				// �I������
				disconnect2(sUser, conn2);
				conn2 = null;
			}

			return alRet;
		}
// MOD 2015.12.15 bevas) ���{ �A���֎~�G���A�@�\�Ή�(is-2�Ǘ��F���x���C���[�W�����) END
// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� START
		/*********************************************************************
		 * �Г��`������X�}�X�^�ꗗ�擾
		 * �����F����b�c
		 * �ߒl�F�X�e�[�^�X�A�ꗗ�i����b�c�A������j...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�Г��`������X�}�X�^�ꗗ�擾�J�n");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			//�c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(CM05F.����b�c) || '|' "
					+     "|| NVL(CM01.�����, ' ') || '|' "
					+     "|| TRIM(CM05F.�X���b�c) || '|' \n"
					+  " FROM �b�l�O�T������X�e CM05F \n"
					+  " LEFT JOIN �b�l�O�P��� CM01 \n"
					+    " ON CM01.����b�c = CM05F.����b�c "
					+    "AND CM01.�폜�e�f = '0' \n"
					+ " WHERE CM05F.�폜�e�f = '0' \n";

				//�����\�����i���͌��������Ȃ��j�́A�Г��`����S�̂��擾
				if(sKey[0].Trim().Length != 0)
				{
					cmdQuery += " AND CM05F.����b�c LIKE '" + sKey[0] + "%' \n";
				}
				cmdQuery += " ORDER BY CM05F.����b�c, CM05F.�X���b�c \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while(reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * �Г��`������X�}�X�^�擾
		 * �����F�W�דX�b�c
		 * �ߒl�F�X�e�[�^�X�A�W�דX�b�c�A�X����...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�Г��`������X�}�X�^�����J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[6];

			//�c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM05F.����b�c "
					+      ", NVL(CM01.�����, ' ') "
					+      ", CM05F.�X���b�c "
					+      ", NVL(CM10.�X����, ' ') "
					+      ", CM05F.�X�V���� \n"
					+  " FROM �b�l�O�T������X�e CM05F \n"
					+  " LEFT JOIN �b�l�O�P��� CM01 \n"
					+    " ON CM01.����b�c = CM05F.����b�c "
					+    "AND CM01.�폜�e�f = '0' \n"
					+  " LEFT JOIN �b�l�P�O�X�� CM10 \n"
					+    " ON CM10.�X���b�c = CM05F.�X���b�c "
					+    "AND CM10.�폜�e�f = '0' \n"
					+ " WHERE CM05F.����b�c = '" + sKey[0] + "' \n"
					+   " AND CM05F.�폜�e�f   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while(reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetDecimal(4).ToString().Trim();
					iCnt++;
				}
				disposeReader(reader);
				reader = null;
				if(iCnt == 1)
				{
					sRet[0] = "�Y���f�[�^������܂���";
				}
				else
				{
					sRet[0] = "����I��";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * �Г��`������X�}�X�^�ǉ�
		 * �����F�W�דX�b�c�A�W��X�b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�Г��`������X�}�X�^�ǉ��J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string s����b�c = sKey[0];
			string s�X���b�c = sKey[1];
			string s���p�҂b�c = sKey[3];

			//�c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT �폜�e�f "
					+   "FROM �b�l�O�T������X�e "
					+  "WHERE ����b�c = '" + s����b�c + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s�폜�e�f = "";
				while (reader.Read())
				{
					s�폜�e�f = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//�ǉ�
					cmdQuery
						= "INSERT INTO �b�l�O�T������X�e \n"
						+ " VALUES ("
						+ " '" + s����b�c + "' " 
						+ ",'" + s�X���b�c + "' "
						+ ",'0' "
						+ "," + s�X�V����
						+ ",'���X�o�^' "
						+ ",'" + s���p�҂b�c + "' "
						+ "," + s�X�V����
						+ ",'�Г`�o�^' "
						+ ",'" + s���p�҂b�c + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					//�ǉ��X�V
					if (s�폜�e�f.Equals("1"))
					{
						cmdQuery
							= "UPDATE �b�l�O�T������X�e \n"
							+   " SET �X���b�c = '" + s�X���b�c + "' "
							+       ",�폜�e�f = '0'"
							+       ",�o�^���� = " + s�X�V����
							+       ",�o�^�o�f = '���X�o�^'"
							+       ",�o�^�� = '" + s���p�҂b�c + "' "
							+       ",�X�V���� = " + s�X�V����
							+       ",�X�V�o�f = '�Г`�o�^' "
							+       ",�X�V�� = '" + s���p�҂b�c + "' \n"
							+ " WHERE ����b�c = '" + s����b�c + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "����I��";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "���ɓo�^����Ă��܂�";
					}
				}
				disposeReader(reader);
				reader = null;
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * �Г��`������X�}�X�^�X�V
		 * �����F�W�דX�b�c�A�W��X�b�c...
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�Г��`������X�}�X�^�X�V�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s�X�V���� = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string sKey����b�c = sKey[0];
			string s�X���b�c = sKey[1];
			string sKey�X�V���� = sKey[2];
			string s���p�҂b�c = sKey[3];

			//�c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�T������X�e \n"
					+   " SET �X���b�c = '" + s�X���b�c + "' "
					+       ",�X�V���� =  " + s�X�V����
					+       ",�X�V�o�f = '���X�X�V' "
					+       ",�X�V�� = '" + s���p�҂b�c + "' \n"
					+ " WHERE ����b�c = '" + sKey����b�c + "' \n"
					+   " AND �폜�e�f = '0' \n"
					+   " AND �X�V���� = " + sKey�X�V���� + " \n";

				if(CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * �Г��`������X�}�X�^�폜
		 * �����F�W�דX�b�c�A�X�V�����A�X�V��
		 * �ߒl�F�X�e�[�^�X
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "�Г��`������X�}�X�^�폜�J�n");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string sKey����b�c = sKey[0];
			string sKey�X�V���� = sKey[1];
			string s���p�҂b�c = sKey[2];

			// �c�a�ڑ�
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "�c�a�ڑ��G���[";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE �b�l�O�T������X�e \n"
					+    "SET �폜�e�f = '1' " 
					+       ",�X�V���� = TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') "
					+       ",�X�V�o�f = '���X�폜' "
					+       ",�X�V�� = '" + s���p�҂b�c + "' "
					+  "WHERE ����b�c = '" + sKey����b�c + "' "
					+    "AND �X�V���� = " + sKey�X�V���� + " \n";

				if(CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "����I��";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "���̒[���ōX�V����Ă��܂�";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				tran.Rollback();
				sRet[0] = "�T�[�o�G���[�F" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

// MOD 2016.04.08 bevas) ���{ �Г��`�[�@�\�ǉ��Ή� END
	}
}
