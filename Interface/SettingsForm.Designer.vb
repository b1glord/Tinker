<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SettingsForm))
        Me.tipNormal = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtMapPath = New System.Windows.Forms.TextBox()
        Me.txtProgramPath = New System.Windows.Forms.TextBox()
        Me.txtCdKeyOwner = New System.Windows.Forms.TextBox()
        Me.txtCommandPrefix = New System.Windows.Forms.TextBox()
        Me.numTickPeriod = New System.Windows.Forms.NumericUpDown()
        Me.numLagLimit = New System.Windows.Forms.NumericUpDown()
        Me.txtNewProfileName = New System.Windows.Forms.TextBox()
        Me.txtInGameName = New System.Windows.Forms.TextBox()
        Me.txtInitialPlugins = New System.Windows.Forms.TextBox()
        Me.txtPortPool = New System.Windows.Forms.TextBox()
        Me.txtBnlsServer = New System.Windows.Forms.TextBox()
        Me.txtGreeting = New System.Windows.Forms.TextBox()
        Me.numReplayBuildNumber = New System.Windows.Forms.NumericUpDown()
        Me.tabsSettings = New System.Windows.Forms.TabControl()
        Me.tabGlobalSettings = New System.Windows.Forms.TabPage()
        Me.lblReplayBuildNumber = New System.Windows.Forms.Label()
        Me.btnLoadReplayBuildNumber = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblWar3FolderPathError = New System.Windows.Forms.Label()
        Me.lblGreeting = New System.Windows.Forms.Label()
        Me.lblBnlsServer = New System.Windows.Forms.Label()
        Me.lblPortPool = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblLagLimit = New System.Windows.Forms.Label()
        Me.lblTickPeriod = New System.Windows.Forms.Label()
        Me.lblCommandPrefix = New System.Windows.Forms.Label()
        Me.lblOwner = New System.Windows.Forms.Label()
        Me.lblMapPath = New System.Windows.Forms.Label()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.tabPlugins = New System.Windows.Forms.TabPage()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnImportPlugin = New System.Windows.Forms.Button()
        Me.gridPlugins = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAccess = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSettings = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tabNewProfile = New System.Windows.Forms.TabPage()
        Me.btnCreateNewProfile = New System.Windows.Forms.Button()
        Me.lblNewProfileName = New System.Windows.Forms.Label()
        Me.btnPluginsHelp = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnUserHelp = New System.Windows.Forms.Button()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        CType(Me.numTickPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numLagLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numReplayBuildNumber, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabsSettings.SuspendLayout()
        Me.tabGlobalSettings.SuspendLayout()
        Me.tabPlugins.SuspendLayout()
        CType(Me.gridPlugins, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabNewProfile.SuspendLayout()
        Me.SuspendLayout()
        '
        'tipNormal
        '
        Me.tipNormal.AutoPopDelay = 1000000
        Me.tipNormal.InitialDelay = 1
        Me.tipNormal.ReshowDelay = 1
        Me.tipNormal.ShowAlways = True
        '
        'txtMapPath
        '
        Me.txtMapPath.Location = New System.Drawing.Point(6, 97)
        Me.txtMapPath.Name = "txtMapPath"
        Me.txtMapPath.Size = New System.Drawing.Size(446, 20)
        Me.txtMapPath.TabIndex = 36
        Me.tipNormal.SetToolTip(Me.txtMapPath, "The location of the folder where maps are stored." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Default: (program files)\Warcr" & _
                "aft III\Maps\")
        '
        'txtProgramPath
        '
        Me.txtProgramPath.Location = New System.Drawing.Point(6, 58)
        Me.txtProgramPath.Name = "txtProgramPath"
        Me.txtProgramPath.Size = New System.Drawing.Size(446, 20)
        Me.txtProgramPath.TabIndex = 34
        Me.tipNormal.SetToolTip(Me.txtProgramPath, "The location of the folder containing the hash files (war3.exe, storm.dll, game.d" & _
                "ll)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Default: (program files)\Warcraft III\")
        '
        'txtCdKeyOwner
        '
        Me.txtCdKeyOwner.Location = New System.Drawing.Point(6, 136)
        Me.txtCdKeyOwner.Name = "txtCdKeyOwner"
        Me.txtCdKeyOwner.Size = New System.Drawing.Size(220, 20)
        Me.txtCdKeyOwner.TabIndex = 40
        Me.tipNormal.SetToolTip(Me.txtCdKeyOwner, "The name displayed to people when they try to logon using a cd key already in use" & _
                ".")
        '
        'txtCommandPrefix
        '
        Me.txtCommandPrefix.Location = New System.Drawing.Point(6, 19)
        Me.txtCommandPrefix.Name = "txtCommandPrefix"
        Me.txtCommandPrefix.Size = New System.Drawing.Size(220, 20)
        Me.txtCommandPrefix.TabIndex = 46
        Me.tipNormal.SetToolTip(Me.txtCommandPrefix, "Prefix that indicates following text is a command.")
        '
        'numTickPeriod
        '
        Me.numTickPeriod.Location = New System.Drawing.Point(6, 175)
        Me.numTickPeriod.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numTickPeriod.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numTickPeriod.Name = "numTickPeriod"
        Me.numTickPeriod.Size = New System.Drawing.Size(220, 20)
        Me.numTickPeriod.TabIndex = 48
        Me.tipNormal.SetToolTip(Me.numTickPeriod, "Determines the game tick period in milliseconds." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Default value for bnet is 250, " & _
                "lan is 100." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Lower values decrease expected command latency, but increase networ" & _
                "k traffic.")
        Me.numTickPeriod.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'numLagLimit
        '
        Me.numLagLimit.Location = New System.Drawing.Point(232, 175)
        Me.numLagLimit.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.numLagLimit.Name = "numLagLimit"
        Me.numLagLimit.Size = New System.Drawing.Size(220, 20)
        Me.numLagLimit.TabIndex = 50
        Me.tipNormal.SetToolTip(Me.numLagLimit, resources.GetString("numLagLimit.ToolTip"))
        '
        'txtNewProfileName
        '
        Me.txtNewProfileName.Location = New System.Drawing.Point(6, 19)
        Me.txtNewProfileName.Name = "txtNewProfileName"
        Me.txtNewProfileName.Size = New System.Drawing.Size(220, 20)
        Me.txtNewProfileName.TabIndex = 48
        Me.txtNewProfileName.Text = "New Profile"
        Me.tipNormal.SetToolTip(Me.txtNewProfileName, "The name to assign to the new profile.")
        '
        'txtInGameName
        '
        Me.txtInGameName.Location = New System.Drawing.Point(232, 136)
        Me.txtInGameName.MaxLength = 15
        Me.txtInGameName.Name = "txtInGameName"
        Me.txtInGameName.Size = New System.Drawing.Size(220, 20)
        Me.txtInGameName.TabIndex = 52
        Me.tipNormal.SetToolTip(Me.txtInGameName, resources.GetString("txtInGameName.ToolTip"))
        '
        'txtInitialPlugins
        '
        Me.txtInitialPlugins.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtInitialPlugins.Location = New System.Drawing.Point(6, 374)
        Me.txtInitialPlugins.MaxLength = 15
        Me.txtInitialPlugins.Name = "txtInitialPlugins"
        Me.txtInitialPlugins.Size = New System.Drawing.Size(271, 20)
        Me.txtInitialPlugins.TabIndex = 56
        Me.tipNormal.SetToolTip(Me.txtInitialPlugins, "The names of plugins to load when the bot starts." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Separate names with a semi-col" & _
                "on, like this:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "plugin1;plugin2;plugin3")
        '
        'txtPortPool
        '
        Me.txtPortPool.Location = New System.Drawing.Point(6, 214)
        Me.txtPortPool.MaxLength = 15
        Me.txtPortPool.Name = "txtPortPool"
        Me.txtPortPool.Size = New System.Drawing.Size(220, 20)
        Me.txtPortPool.TabIndex = 54
        Me.tipNormal.SetToolTip(Me.txtPortPool, resources.GetString("txtPortPool.ToolTip"))
        '
        'txtBnlsServer
        '
        Me.txtBnlsServer.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBnlsServer.Location = New System.Drawing.Point(232, 19)
        Me.txtBnlsServer.Name = "txtBnlsServer"
        Me.txtBnlsServer.Size = New System.Drawing.Size(220, 20)
        Me.txtBnlsServer.TabIndex = 60
        Me.tipNormal.SetToolTip(Me.txtBnlsServer, resources.GetString("txtBnlsServer.ToolTip"))
        '
        'txtGreeting
        '
        Me.txtGreeting.Location = New System.Drawing.Point(6, 253)
        Me.txtGreeting.Multiline = True
        Me.txtGreeting.Name = "txtGreeting"
        Me.txtGreeting.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtGreeting.Size = New System.Drawing.Size(446, 65)
        Me.txtGreeting.TabIndex = 62
        Me.tipNormal.SetToolTip(Me.txtGreeting, "The text sent to players as they join a game." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Leave blank for no greeting." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Line" & _
                "s longer than 220 characters will be split.")
        '
        'numReplayBuildNumber
        '
        Me.numReplayBuildNumber.Location = New System.Drawing.Point(232, 215)
        Me.numReplayBuildNumber.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.numReplayBuildNumber.Name = "numReplayBuildNumber"
        Me.numReplayBuildNumber.Size = New System.Drawing.Size(119, 20)
        Me.numReplayBuildNumber.TabIndex = 65
        Me.tipNormal.SetToolTip(Me.numReplayBuildNumber, resources.GetString("numReplayBuildNumber.ToolTip"))
        '
        'tabsSettings
        '
        Me.tabsSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabsSettings.Controls.Add(Me.tabGlobalSettings)
        Me.tabsSettings.Controls.Add(Me.tabPlugins)
        Me.tabsSettings.Controls.Add(Me.tabNewProfile)
        Me.tabsSettings.Location = New System.Drawing.Point(12, 12)
        Me.tabsSettings.Name = "tabsSettings"
        Me.tabsSettings.SelectedIndex = 0
        Me.tabsSettings.Size = New System.Drawing.Size(481, 427)
        Me.tabsSettings.TabIndex = 30
        '
        'tabGlobalSettings
        '
        Me.tabGlobalSettings.AutoScroll = True
        Me.tabGlobalSettings.Controls.Add(Me.lblReplayBuildNumber)
        Me.tabGlobalSettings.Controls.Add(Me.btnLoadReplayBuildNumber)
        Me.tabGlobalSettings.Controls.Add(Me.numReplayBuildNumber)
        Me.tabGlobalSettings.Controls.Add(Me.Label4)
        Me.tabGlobalSettings.Controls.Add(Me.lblWar3FolderPathError)
        Me.tabGlobalSettings.Controls.Add(Me.lblGreeting)
        Me.tabGlobalSettings.Controls.Add(Me.txtGreeting)
        Me.tabGlobalSettings.Controls.Add(Me.txtBnlsServer)
        Me.tabGlobalSettings.Controls.Add(Me.lblBnlsServer)
        Me.tabGlobalSettings.Controls.Add(Me.lblPortPool)
        Me.tabGlobalSettings.Controls.Add(Me.txtPortPool)
        Me.tabGlobalSettings.Controls.Add(Me.Label1)
        Me.tabGlobalSettings.Controls.Add(Me.txtInGameName)
        Me.tabGlobalSettings.Controls.Add(Me.numLagLimit)
        Me.tabGlobalSettings.Controls.Add(Me.lblLagLimit)
        Me.tabGlobalSettings.Controls.Add(Me.numTickPeriod)
        Me.tabGlobalSettings.Controls.Add(Me.lblTickPeriod)
        Me.tabGlobalSettings.Controls.Add(Me.lblCommandPrefix)
        Me.tabGlobalSettings.Controls.Add(Me.txtCommandPrefix)
        Me.tabGlobalSettings.Controls.Add(Me.lblOwner)
        Me.tabGlobalSettings.Controls.Add(Me.txtCdKeyOwner)
        Me.tabGlobalSettings.Controls.Add(Me.lblMapPath)
        Me.tabGlobalSettings.Controls.Add(Me.txtMapPath)
        Me.tabGlobalSettings.Controls.Add(Me.txtProgramPath)
        Me.tabGlobalSettings.Controls.Add(Me.lblPath)
        Me.tabGlobalSettings.Location = New System.Drawing.Point(4, 22)
        Me.tabGlobalSettings.Name = "tabGlobalSettings"
        Me.tabGlobalSettings.Size = New System.Drawing.Size(473, 401)
        Me.tabGlobalSettings.TabIndex = 3
        Me.tabGlobalSettings.Text = "Global"
        Me.tabGlobalSettings.UseVisualStyleBackColor = True
        '
        'lblReplayBuildNumber
        '
        Me.lblReplayBuildNumber.AutoSize = True
        Me.lblReplayBuildNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReplayBuildNumber.ForeColor = System.Drawing.Color.Red
        Me.lblReplayBuildNumber.Location = New System.Drawing.Point(352, 198)
        Me.lblReplayBuildNumber.Name = "lblReplayBuildNumber"
        Me.lblReplayBuildNumber.Size = New System.Drawing.Size(64, 13)
        Me.lblReplayBuildNumber.TabIndex = 68
        Me.lblReplayBuildNumber.Text = "Default: #"
        Me.lblReplayBuildNumber.Visible = False
        '
        'btnLoadReplayBuildNumber
        '
        Me.btnLoadReplayBuildNumber.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadReplayBuildNumber.Location = New System.Drawing.Point(355, 213)
        Me.btnLoadReplayBuildNumber.Name = "btnLoadReplayBuildNumber"
        Me.btnLoadReplayBuildNumber.Size = New System.Drawing.Size(97, 21)
        Me.btnLoadReplayBuildNumber.TabIndex = 67
        Me.btnLoadReplayBuildNumber.Text = "From Replay ..."
        Me.btnLoadReplayBuildNumber.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(229, 199)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(106, 13)
        Me.Label4.TabIndex = 66
        Me.Label4.Text = "Replay Build Number"
        '
        'lblWar3FolderPathError
        '
        Me.lblWar3FolderPathError.AutoSize = True
        Me.lblWar3FolderPathError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWar3FolderPathError.ForeColor = System.Drawing.Color.Red
        Me.lblWar3FolderPathError.Location = New System.Drawing.Point(386, 42)
        Me.lblWar3FolderPathError.Name = "lblWar3FolderPathError"
        Me.lblWar3FolderPathError.Size = New System.Drawing.Size(66, 13)
        Me.lblWar3FolderPathError.TabIndex = 64
        Me.lblWar3FolderPathError.Text = "Not Found"
        Me.lblWar3FolderPathError.Visible = False
        '
        'lblGreeting
        '
        Me.lblGreeting.AutoSize = True
        Me.lblGreeting.Location = New System.Drawing.Point(3, 237)
        Me.lblGreeting.Name = "lblGreeting"
        Me.lblGreeting.Size = New System.Drawing.Size(82, 13)
        Me.lblGreeting.TabIndex = 63
        Me.lblGreeting.Text = "Default greeting"
        '
        'lblBnlsServer
        '
        Me.lblBnlsServer.AutoSize = True
        Me.lblBnlsServer.Location = New System.Drawing.Point(229, 3)
        Me.lblBnlsServer.Name = "lblBnlsServer"
        Me.lblBnlsServer.Size = New System.Drawing.Size(161, 13)
        Me.lblBnlsServer.TabIndex = 61
        Me.lblBnlsServer.Text = "BattleNet Login Server (Warden)"
        '
        'lblPortPool
        '
        Me.lblPortPool.AutoSize = True
        Me.lblPortPool.Location = New System.Drawing.Point(3, 198)
        Me.lblPortPool.Name = "lblPortPool"
        Me.lblPortPool.Size = New System.Drawing.Size(50, 13)
        Me.lblPortPool.TabIndex = 55
        Me.lblPortPool.Text = "Port Pool"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(229, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 53
        Me.Label1.Text = "In-game name"
        '
        'lblLagLimit
        '
        Me.lblLagLimit.AutoSize = True
        Me.lblLagLimit.Location = New System.Drawing.Point(229, 159)
        Me.lblLagLimit.Name = "lblLagLimit"
        Me.lblLagLimit.Size = New System.Drawing.Size(117, 13)
        Me.lblLagLimit.TabIndex = 51
        Me.lblLagLimit.Text = "Default Game Lag Limit"
        '
        'lblTickPeriod
        '
        Me.lblTickPeriod.AutoSize = True
        Me.lblTickPeriod.Location = New System.Drawing.Point(3, 159)
        Me.lblTickPeriod.Name = "lblTickPeriod"
        Me.lblTickPeriod.Size = New System.Drawing.Size(129, 13)
        Me.lblTickPeriod.TabIndex = 49
        Me.lblTickPeriod.Text = "Default Game Tick Period"
        '
        'lblCommandPrefix
        '
        Me.lblCommandPrefix.AutoSize = True
        Me.lblCommandPrefix.Location = New System.Drawing.Point(3, 3)
        Me.lblCommandPrefix.Name = "lblCommandPrefix"
        Me.lblCommandPrefix.Size = New System.Drawing.Size(83, 13)
        Me.lblCommandPrefix.TabIndex = 47
        Me.lblCommandPrefix.Text = "Command Prefix"
        '
        'lblOwner
        '
        Me.lblOwner.AutoSize = True
        Me.lblOwner.Location = New System.Drawing.Point(3, 120)
        Me.lblOwner.Name = "lblOwner"
        Me.lblOwner.Size = New System.Drawing.Size(77, 13)
        Me.lblOwner.TabIndex = 41
        Me.lblOwner.Text = "CD Key Owner"
        '
        'lblMapPath
        '
        Me.lblMapPath.AutoSize = True
        Me.lblMapPath.Location = New System.Drawing.Point(3, 81)
        Me.lblMapPath.Name = "lblMapPath"
        Me.lblMapPath.Size = New System.Drawing.Size(53, 13)
        Me.lblMapPath.TabIndex = 37
        Me.lblMapPath.Text = "Map Path"
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(3, 42)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(114, 13)
        Me.lblPath.TabIndex = 35
        Me.lblPath.Text = "Warcraft 3 Folder Path"
        '
        'tabPlugins
        '
        Me.tabPlugins.Controls.Add(Me.Label3)
        Me.tabPlugins.Controls.Add(Me.Label2)
        Me.tabPlugins.Controls.Add(Me.txtInitialPlugins)
        Me.tabPlugins.Controls.Add(Me.btnImportPlugin)
        Me.tabPlugins.Controls.Add(Me.gridPlugins)
        Me.tabPlugins.Location = New System.Drawing.Point(4, 22)
        Me.tabPlugins.Name = "tabPlugins"
        Me.tabPlugins.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPlugins.Size = New System.Drawing.Size(473, 401)
        Me.tabPlugins.TabIndex = 4
        Me.tabPlugins.Text = "Plugins"
        Me.tabPlugins.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "Available Plugins"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 358)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 13)
        Me.Label2.TabIndex = 57
        Me.Label2.Text = "Plugins loaded at startup"
        '
        'btnImportPlugin
        '
        Me.btnImportPlugin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImportPlugin.Location = New System.Drawing.Point(370, 369)
        Me.btnImportPlugin.Name = "btnImportPlugin"
        Me.btnImportPlugin.Size = New System.Drawing.Size(97, 29)
        Me.btnImportPlugin.TabIndex = 37
        Me.btnImportPlugin.Text = "Import Plugin"
        Me.btnImportPlugin.UseVisualStyleBackColor = True
        '
        'gridPlugins
        '
        Me.gridPlugins.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPlugins.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colAccess, Me.colSettings})
        Me.gridPlugins.Location = New System.Drawing.Point(0, 19)
        Me.gridPlugins.Name = "gridPlugins"
        Me.gridPlugins.Size = New System.Drawing.Size(473, 336)
        Me.gridPlugins.TabIndex = 33
        '
        'colName
        '
        Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colName.FillWeight = 50.0!
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        '
        'colAccess
        '
        Me.colAccess.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colAccess.HeaderText = "Location"
        Me.colAccess.Name = "colAccess"
        '
        'colSettings
        '
        Me.colSettings.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colSettings.HeaderText = "Arguments"
        Me.colSettings.Name = "colSettings"
        Me.colSettings.Visible = False
        '
        'tabNewProfile
        '
        Me.tabNewProfile.AutoScroll = True
        Me.tabNewProfile.Controls.Add(Me.btnCreateNewProfile)
        Me.tabNewProfile.Controls.Add(Me.lblNewProfileName)
        Me.tabNewProfile.Controls.Add(Me.txtNewProfileName)
        Me.tabNewProfile.Location = New System.Drawing.Point(4, 22)
        Me.tabNewProfile.Name = "tabNewProfile"
        Me.tabNewProfile.Padding = New System.Windows.Forms.Padding(3)
        Me.tabNewProfile.Size = New System.Drawing.Size(473, 401)
        Me.tabNewProfile.TabIndex = 0
        Me.tabNewProfile.Text = "[ New Profile ... ]"
        Me.tabNewProfile.UseVisualStyleBackColor = True
        '
        'btnCreateNewProfile
        '
        Me.btnCreateNewProfile.Location = New System.Drawing.Point(6, 45)
        Me.btnCreateNewProfile.Name = "btnCreateNewProfile"
        Me.btnCreateNewProfile.Size = New System.Drawing.Size(220, 29)
        Me.btnCreateNewProfile.TabIndex = 50
        Me.btnCreateNewProfile.Text = "Create New Profile"
        Me.btnCreateNewProfile.UseVisualStyleBackColor = True
        '
        'lblNewProfileName
        '
        Me.lblNewProfileName.AutoSize = True
        Me.lblNewProfileName.Location = New System.Drawing.Point(3, 3)
        Me.lblNewProfileName.Name = "lblNewProfileName"
        Me.lblNewProfileName.Size = New System.Drawing.Size(92, 13)
        Me.lblNewProfileName.TabIndex = 49
        Me.lblNewProfileName.Text = "New Profile Name"
        '
        'btnPluginsHelp
        '
        Me.btnPluginsHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPluginsHelp.Location = New System.Drawing.Point(115, 445)
        Me.btnPluginsHelp.Name = "btnPluginsHelp"
        Me.btnPluginsHelp.Size = New System.Drawing.Size(97, 29)
        Me.btnPluginsHelp.TabIndex = 36
        Me.btnPluginsHelp.Text = "Plugins Help"
        Me.btnPluginsHelp.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(396, 445)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(97, 29)
        Me.btnCancel.TabIndex = 29
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(293, 445)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(97, 29)
        Me.btnSave.TabIndex = 28
        Me.btnSave.Text = "Apply"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnUserHelp
        '
        Me.btnUserHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUserHelp.Location = New System.Drawing.Point(12, 445)
        Me.btnUserHelp.Name = "btnUserHelp"
        Me.btnUserHelp.Size = New System.Drawing.Size(97, 29)
        Me.btnUserHelp.TabIndex = 35
        Me.btnUserHelp.Text = "Users Help"
        Me.btnUserHelp.UseVisualStyleBackColor = True
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.Title = "Select Plugin"
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(505, 486)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnUserHelp)
        Me.Controls.Add(Me.tabsSettings)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnPluginsHelp)
        Me.Name = "SettingsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        CType(Me.numTickPeriod, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numLagLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numReplayBuildNumber, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabsSettings.ResumeLayout(False)
        Me.tabGlobalSettings.ResumeLayout(False)
        Me.tabGlobalSettings.PerformLayout()
        Me.tabPlugins.ResumeLayout(False)
        Me.tabPlugins.PerformLayout()
        CType(Me.gridPlugins, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabNewProfile.ResumeLayout(False)
        Me.tabNewProfile.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tipNormal As System.Windows.Forms.ToolTip
    Friend WithEvents tabsSettings As System.Windows.Forms.TabControl
    Friend WithEvents tabNewProfile As System.Windows.Forms.TabPage
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnUserHelp As System.Windows.Forms.Button
    Friend WithEvents tabGlobalSettings As System.Windows.Forms.TabPage
    Friend WithEvents lblMapPath As System.Windows.Forms.Label
    Friend WithEvents txtMapPath As System.Windows.Forms.TextBox
    Friend WithEvents txtProgramPath As System.Windows.Forms.TextBox
    Friend WithEvents lblPath As System.Windows.Forms.Label
    Friend WithEvents lblOwner As System.Windows.Forms.Label
    Friend WithEvents txtCdKeyOwner As System.Windows.Forms.TextBox
    Friend WithEvents lblCommandPrefix As System.Windows.Forms.Label
    Friend WithEvents txtCommandPrefix As System.Windows.Forms.TextBox
    Friend WithEvents numTickPeriod As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblTickPeriod As System.Windows.Forms.Label
    Friend WithEvents numLagLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblLagLimit As System.Windows.Forms.Label
    Friend WithEvents btnCreateNewProfile As System.Windows.Forms.Button
    Friend WithEvents lblNewProfileName As System.Windows.Forms.Label
    Friend WithEvents txtNewProfileName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtInGameName As System.Windows.Forms.TextBox
    Friend WithEvents tabPlugins As System.Windows.Forms.TabPage
    Friend WithEvents gridPlugins As System.Windows.Forms.DataGridView
    Friend WithEvents btnPluginsHelp As System.Windows.Forms.Button
    Friend WithEvents btnImportPlugin As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAccess As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSettings As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtInitialPlugins As System.Windows.Forms.TextBox
    Friend WithEvents lblPortPool As System.Windows.Forms.Label
    Friend WithEvents txtPortPool As System.Windows.Forms.TextBox
    Friend WithEvents txtBnlsServer As System.Windows.Forms.TextBox
    Friend WithEvents lblBnlsServer As System.Windows.Forms.Label
    Friend WithEvents lblGreeting As System.Windows.Forms.Label
    Friend WithEvents txtGreeting As System.Windows.Forms.TextBox
    Friend WithEvents lblWar3FolderPathError As System.Windows.Forms.Label
    Friend WithEvents btnLoadReplayBuildNumber As System.Windows.Forms.Button
    Friend WithEvents numReplayBuildNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblReplayBuildNumber As System.Windows.Forms.Label
End Class
